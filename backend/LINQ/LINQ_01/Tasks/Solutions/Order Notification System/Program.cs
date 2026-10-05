namespace Order_Notification_System
{
    internal class Program
    {
        class Order
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public decimal TotalPrice { get; set; }
        }

        delegate void NotificationHandler(Order order);

        static void Main(string[] args)
        {

            static void SendEmail ( Order order)
            {
                Console.WriteLine( $"email sent to { order.CustomerName} for order #{order.Id}" );
            }

            static void SendSMS( Order order )
            {
                Console.WriteLine($"SMS sent to { order.CustomerName } for order #{order.Id}");
            }

            static void SendNotification ( Order order)
            {
                Console.WriteLine( $"Notification sent to { order.CustomerName} for order #{order.Id}");
            }

            static void ProcessOrder ( Order order , NotificationHandler orderHandler)
            {
                Console.WriteLine(order.Id);
                Console.WriteLine(order.TotalPrice);
                orderHandler(order);
            }

            Order o1 = new Order
            {
                CustomerName = "dsadsa",
                TotalPrice = 100,
                Id = 1
            };
            ProcessOrder(o1, SendNotification);
        }
    }
}
