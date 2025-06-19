import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ClientsTable from '../components/ClientsTable';
import RateForm from '../components/RateForm';
import PaymentsHistory from '../components/PaymentsHistory';

export default function Dashboard() {
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) navigate('/');
  }, [navigate]);

  const logout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  return (
    <div className="dashboard">
      <div className="dashboard-header">
        <h1>Admin Dashboard</h1>
        <button onClick={logout} className="logout-btn">Logout</button>
      </div>
      <div className="dashboard-content">
        <ClientsTable />
        <RateForm />
        <PaymentsHistory />
      </div>
    </div>
  );
}