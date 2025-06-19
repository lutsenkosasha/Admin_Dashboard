import { useEffect, useState } from 'react';
import api from '../api';

export default function RateForm() {
  const [rate, setRate] = useState(0);
  const [newRate, setNewRate] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get('/rate')
      .then((res) => setRate(res.data))
      .finally(() => setLoading(false));
  }, []);

  const updateRate = async () => {
    try {
      await api.post('/rate', { value: parseFloat(newRate) });
      setRate({ ...rate, tokenRate: newRate });
      setNewRate('');
    } catch {
      alert('Failed to update rate');
    }
  };

  if (loading) return <p>Loading rate...</p>;

  return (
    <div className="rate-form">
      <h3>Token Rate: {rate.tokenRate}</h3>
      <input
        type="number"
        placeholder="New rate"
        value={newRate}
        onChange={(e) => setNewRate(e.target.value)}
      />
      <button onClick={updateRate}>Update</button>
    </div>
  );
}