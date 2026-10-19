using MauiMissionTable.Events;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class EventBusTests
    {
        [Fact]
        public void Publish_CallsSubscribedHandler()
        {
            var bus = new EventBus();
            bool called = false;

            bus.Subscribe<TickEvent>(e => called = true);

            bus.Publish(new TickEvent(DateTime.UtcNow));

            Assert.True(called);
        }

        [Fact]
        public void MultipleHandlers_AllAreCalled()
        {
            var bus = new EventBus();
            int callCount = 0;

            bus.Subscribe<TickEvent>(e => callCount++);
            bus.Subscribe<TickEvent>(e => callCount++);

            bus.Publish(new TickEvent(DateTime.UtcNow));

            Assert.Equal(2, callCount);
        }

        [Fact]
        public void Publish_DoesNotThrow_WhenNoSubscribers()
        {
            var bus = new EventBus();

            var ex = Record.Exception(() => bus.Publish(new TickEvent(DateTime.UtcNow)));

            Assert.Null(ex);
        }

        [Fact(Timeout = 20000)]
        public async Task ManySubscribersAndPublishers_NoDeadlock()
        {
            var bus = new EventBus();
            int subscribers = 100;
            int publishers = 10;
            int eventsPerPublisher = 100;

            for (int i = 0; i < subscribers; i++)
                bus.Subscribe<TickEvent>(_ => { /* empty */ });

            var tasks = new List<Task>();
            for (int p = 0; p < publishers; p++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int i = 0; i < eventsPerPublisher; i++)
                        bus.Publish(new TickEvent(DateTime.UtcNow));
                }));
            }

            await Task.WhenAll(tasks);

            Assert.True(true); // reached without deadlock
        }

        [Fact(Timeout = 10000)]
        public async Task ManyThreads_NoLostEvents()
        {
            var bus = new EventBus();
            int count = 0;
            int publishers = 10;
            int eventsPerPublisher = 500;

            bus.Subscribe<TickEvent>(_ => Interlocked.Increment(ref count));

            var tasks = new List<Task>();
            for (int p = 0; p < publishers; p++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int i = 0; i < eventsPerPublisher; i++)
                        bus.Publish(new TickEvent(DateTime.UtcNow));
                }));
            }

            await Task.WhenAll(tasks);

            Assert.Equal(publishers * eventsPerPublisher, count);
        }
    }
}
