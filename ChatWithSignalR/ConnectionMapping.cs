using System.Collections.Generic;
using System.Linq;

namespace ChatWithSignalR
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }
        //Bağlantı kuran kişilerin connectionIdlerini bir dictionary'e kaydederek sistemde bulunup bulunmadığını kontrol eder. Aynı zamanda her bir yeniden bağlantıda oluşan connectionid'leri value listesine ekleyerek kullanıcıya ait connection id listesi tutar.
        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

    }
}