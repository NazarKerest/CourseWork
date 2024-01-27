//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace CourseWork
//{
//	public class ConsoleServer
//	{
//		private const string ServerIp = "127.0.0.1";
//		private const string Password = "password"; // Встановіть власний пароль

//		static void Main2()
//		{
//			TcpListener server = new TcpListener(IPAddress.Parse(ServerIp), 12345);
//			server.Start();

//			Console.WriteLine("Сервер запущено...");

//			while (true)
//			{
//				TcpClient client = server.AcceptTcpClient();
//				Console.WriteLine("Клієнт підключено.");

//				HandleClient(client);
//			}
//		}

//		static void HandleClient(TcpClient client)
//		{
//			NetworkStream stream = client.GetStream();

//			// Отримання інформації для аутентифікації
//			byte[] authInfoBuffer = new byte[1024];
//			int bytesRead = stream.Read(authInfoBuffer, 0, authInfoBuffer.Length);
//			string authInfo = Encoding.UTF8.GetString(authInfoBuffer, 0, bytesRead);

//			if (authInfo == Password)
//			{
//				stream.WriteByte(1); // Відправлення дозволу на завантаження
//				Console.WriteLine("Клієнт аутентифікований.");
//			}
//			else
//			{
//				stream.WriteByte(0); // Відправлення відмови у доступі
//				Console.WriteLine("Клієнт не аутентифікований. З'єднання закрито.");
//				client.Close();
//				return;
//			}

//			// Отримання інформації про файл
//			byte[] fileInfoBuffer = new byte[1024];
//			bytesRead = stream.Read(fileInfoBuffer, 0, fileInfoBuffer.Length);
//			string fileInfo = Encoding.UTF8.GetString(fileInfoBuffer, 0, bytesRead);
//			string[] fileDetails = fileInfo.Split(';');
//			string fileName = fileDetails[0];
//			long fileSize = long.Parse(fileDetails[1]);

//			Console.WriteLine($"Отримано інформацію про файл: {fileName}, розмір {fileSize} байт.");

//			// Отримання та збереження файлу
//			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
//			{
//				byte[] buffer = new byte[1024];
//				long remainingBytes = fileSize;
//				while (remainingBytes > 0)
//				{
//					bytesRead = stream.Read(buffer, 0, buffer.Length);
//					fileStream.Write(buffer, 0, bytesRead);
//					remainingBytes -= bytesRead;
//				}
//			}

//			Console.WriteLine($"Файл '{fileName}' успішно отримано та збережено.");

//			// Закриття ресурсів
//			stream.Close();
//			client.Close();
//		}
//	}
//}
