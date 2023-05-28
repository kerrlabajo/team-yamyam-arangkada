import { Divider, List, ListItem, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { Logout, Person, Dashboard, Payment, Commute, People, DriveEta } from '@mui/icons-material';
import PaymentIcon from '@mui/icons-material/Payment';
import { NavbarLink } from './NavbarLink';
import { useContext } from 'react';
import { UserContext, UserContextType } from '../../contexts/UserContext';
import { useNavigate } from 'react-router-dom';

type NavbarListProps = {
  open: boolean,
};

const NavbarList = ({ open }: NavbarListProps) => {
  const { user, handleSetUser } = useContext(UserContext) as UserContextType;
  const navigate = useNavigate();
  
  const operatorList: { text: string, icon: React.ReactNode, link: string, end: boolean }[] = [
    { text: "Dashboard", icon: <Dashboard />, link: "/home", end: true },
    { text: "Vehicles", icon: <Commute />, link: "/vehicles", end: false },
    { text: "Add Vehicles", icon: <DriveEta />, link: "/vehicles/add", end: false },
    { text: "Drivers", icon: <People />, link: "/drivers", end: false },
    { text: "Add Drivers", icon: <People />, link: "/drivers/add", end: false },
    { text: "Transactions", icon: <Payment />, link: "/transactions", end: false },
    { text: "Record Transactions", icon: <PaymentIcon />, link: "/transactions/record", end: false },
  ];

  const handleLogout = () => {
    handleSetUser(null);
    navigate("/", { replace: true });
  }

  return (
    <>
      {/* Main List */}
      <List>
        {(user?.fullName !== null ? operatorList : []).map((listItem, index) => (
          <ListItem key={index} disablePadding sx={{ display: 'block' }} >
            <NavbarLink to={listItem.link} text={listItem.text} icon={listItem.icon} open={open} end={listItem.end} />
          </ListItem>
        ))}
      </List>
      <Divider />

      {/* Secondary List */}
      <List>
        <ListItem disablePadding sx={{ display: 'block' }}>
          <NavbarLink to={user?.fullName !== null ? "/profile" : "" } text="Account" icon={<Person />} open={open} end={false} />
        </ListItem>
        <ListItem disablePadding sx={{ display: 'block' }}>
          <ListItemButton onClick={handleLogout}
            sx={{
              color: "primary.contrastText",
              minHeight: 48,
              justifyContent: open ? 'initial' : 'center', px: 2.5,
              "&.Mui-selected": { backgroundColor: "primary.dark" },
              "&.Mui-selected:hover": { backgroundColor: "primary.dark" },
            }}>
            <ListItemIcon sx={{ color: "primary.contrastText", minWidth: 0, mr: open ? 3 : 'auto', justifyContent: 'center' }}>
              <Logout />
            </ListItemIcon>
            <ListItemText sx={{ opacity: open ? 1 : 0 }} primary="Logout" />
          </ListItemButton>
        </ListItem>
      </List>
    </>
  );
}

export default NavbarList;