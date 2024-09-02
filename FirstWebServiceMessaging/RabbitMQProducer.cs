using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FirstWebServiceMessaging
{
    public class RabbitMQProducer
    {
        public void SendMessage(string message)
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

                var body = Encoding.UTF8.GetBytes(message);  // Mesajı byte dizisine dönüştürür
                channel.BasicPublish(exchange: "",  // Mesajı yayımlayacak bir exchange kullanmaz
                                     routingKey: "userQueue",  // Mesajın gönderileceği kuyruk
                                     basicProperties: null,  // Mesaj özellikleri yok
                                     body: body);  // Gönderilen mesajın içeriği
            }
        }
    }
}
