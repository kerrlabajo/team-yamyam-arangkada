import { Route, Routes } from 'react-router-dom';
import LoginPage from './pages/landing/LoginPage';
import RegisterPage from './pages/landing/RegisterPage';
import VehicleListPage from './pages/vehicle/VehicleListPage';
import AddVehiclePage from './pages/vehicle/AddVehiclePage';
import OperatorProfilePage from './pages/operator/OperatorProfilePage';
import EditOperatorPage from './pages/operator/EditOperatorPage';
import RemoveOperatorPage from './pages/operator/RemoveOperatorPage';
import RemoveVehiclePage from './pages/vehicle/RemoveVehiclePage';
import DriverListPage from './pages/driver/DriverListPage';
import AddDriverPage from './pages/driver/AddDriverPage';
import EditDriverPage from './pages/driver/EditDriverPage';
import TransactionListPage from './pages/transactions/TransactionListPage';
import HomePage from './pages/landing/HomePage';
import EditVehiclePage from './pages/vehicle/EditVehiclePage';
import RemoveDriverPage from './pages/driver/RemoveDriverPage';
import RecordTransactionPage from './pages/transactions/RecordTransactionPage';
import EditTransactionPage from './pages/transactions/EditTransactionPage';
import VerifyEmailPage from './pages/landing/VerifyEmailPage';
import HomeLayout from './components/landing/views/HomeLayout';
import AssignDriverPage from './pages/driver/AssignDriverPage';
import DeleteTransactionPage from './pages/transactions/DeleteTransactionPage';
import ContactUsPage from './pages/landing/ContactUsPage';

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="" element={<LoginPage />} />

      <Route path="contact" element={<ContactUsPage />} />

      <Route path="registration">
        <Route index element={<RegisterPage />} />
        <Route path="verify" element={<VerifyEmailPage />} />
      </Route>

      <Route path="home" element={<HomeLayout />} >
        <Route index element={<HomePage />} />
      </Route>

      <Route path="profile"element={<HomeLayout />} >
        <Route index element={<OperatorProfilePage />} />
        <Route path="edit" element={<EditOperatorPage />} />
        <Route path="delete" element={<RemoveOperatorPage />} />
      </Route>

      <Route path="vehicles" element={<HomeLayout />} >
        <Route index element={<VehicleListPage />} />
        <Route path="add" element={<AddVehiclePage />} />
        <Route path=":id/edit" element={<EditVehiclePage />} />
        <Route path=":id/remove" element={<RemoveVehiclePage />} />
      </Route>

      <Route path="drivers" element={<HomeLayout />} >
        <Route index element={<DriverListPage />} />
        <Route path="add" element={<AddDriverPage />} />
        <Route path=":id/assign" element={<AssignDriverPage />} />
        <Route path=":id/edit" element={<EditDriverPage />} />
        <Route path=":id/remove" element={<RemoveDriverPage />} />
      </Route>

      <Route path="transactions" element={<HomeLayout />} >
        <Route index element={<TransactionListPage />} />
        <Route path="record" element={<RecordTransactionPage />} />
        <Route path=":id/edit" element={<EditTransactionPage />} />
        <Route path=":id/delete" element={<DeleteTransactionPage />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;