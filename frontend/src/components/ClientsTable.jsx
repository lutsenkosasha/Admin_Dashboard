import { useEffect, useState } from 'react';
import api from '../api';

export default function ClientsTable() {
  const [clients, setClients] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get('/clients')
      .then((res) => setClients(res.data))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Loading clients...</p>;

  return (
    <div className="clients-table">
      <h3>Clients</h3>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>BalanceT</th>
          </tr>
        </thead>
        <tbody>
          {clients.map((client) => (
            <tr key={client.id}>
              <td>{client.name}</td>
              <td>{client.email}</td>
              <td>{client.balanceT}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}