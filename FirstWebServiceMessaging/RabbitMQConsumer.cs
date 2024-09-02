using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;

namespace FirstWebServiceMessaging
{
    public class RabbitMQConsumer
    {
        public void RecieveMessage(Action<string> processMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };  // RabbitMQ sunucusuna bağlantı oluşturur
            using (var connection = factory.CreateConnection())  // Bağlantıyı oluşturur
            using (var channel = connection.CreateModel())  // Kanalı oluşturur
            {
                channel.QueueDeclare(queue: "userQueue",  // Kuyruğu tanımlar, kuyruk yoksa oluşturur
                                     durable: false,  // Kuyruk verileri sunucu kapandığında kaybolur
                                     exclusive: false,  // Kuyruk sadece bu bağlantıya özel değildir
                                     autoDelete: false,  // Kuyruk boşaldığında otomatik olarak silinmez
                                     arguments: null);  // Ek argüman yok

                var consumer = new EventingBasicConsumer(channel);  // Bir tüketici oluşturur
                consumer.Received += (model, ea) =>  // Mesaj alındığında tetiklenen olay
                {
                    var body = ea.Body.ToArray();  // Mesajın içeriğini byte dizisine dönüştürür
                    var message = Encoding.UTF8.GetString(body);  // Byte dizisini stringe dönüştürür
                    processMessage(message);  // Mesajı işlemek için verilen delegate/metod çağırılır
                };
                channel.BasicConsume(queue: "userQueue",  // Mesajların alınacağı kuyruk
                                     autoAck: true,  // Mesajın alındığını otomatik olarak onaylar
                                     consumer: consumer);  // Tüketiciyi bağlar
            }
        }
    }
}
