import { useEffect, useState } from 'react';
import api from '../api';

export default function PaymentsHistory() {
  const [payments, setPayments] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get('/payments?take=5')
      .then((res) => setPayments(res.data))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Loading payments...</p>;

  return (
    <div className="payments-history">
      <h3>Latest Payments</h3>
      <ul>
        {payments.map((p) => (
          <li key={p.id}>
            {p.client?.name ?? 'Unknown'} — {p.amount}T — {new Date(p.timestamp).toLocaleString()}
          </li>
        ))}
      </ul>
    </div>
  );
}
