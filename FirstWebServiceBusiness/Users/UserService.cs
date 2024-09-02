using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FirstWebServiceMessaging;
using FirstWebServiceData.Models;
using FirstWebServiceData.Models.Entity;

namespace FirstWebService.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly RabbitMQProducer _rabbitMQProducer;

        public UserService(IUserRepository userRepository, RabbitMQProducer rabbitMQProducer)
        {
            _userRepository = userRepository;
            _rabbitMQProducer = rabbitMQProducer;
        }
        
        public void SendUserToQueqe(User user)
        {
            {
                var message = JsonConvert.SerializeObject(user);
                _rabbitMQProducer.SendMessage(message);

            }
        }
        public void AddUser(User user)
        {
            try
            {
                SendUserToQueqe(user);
            }
            catch (Exception ex)
            {
                // Hata loglama
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                throw;
            }
        }

        public void DeleteUser(int id)
        {
            var message = JsonConvert.SerializeObject(id);
            _rabbitMQProducer.SendMessage(message);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void UpdateUser(User user)
        {
            SendUserToQueqe(user);
        }



    }
}

