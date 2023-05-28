import { Box } from '@mui/material';
import { useState } from 'react';
import { DrawerHeader } from '../../../styles/NavbarStyles';
import Navbar from '../../navigation/Navbar';
import Topbar from '../../shared/Topbar';
import { Outlet } from 'react-router-dom';

const HomeLayout = () => {
  const [open, setOpen] = useState(false);

  const handleDrawerOpen = () => {
    setOpen(true);
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };

  return (
    <Box sx={{ display: 'flex' }}>

      <Topbar open={open} handleDrawerOpen={handleDrawerOpen} />

      <Navbar open={open} handleDrawerClose={handleDrawerClose} />

      <Box component="main" sx={{ flexGrow: 1, padding: "0 10%" }}>
        <DrawerHeader />
          <Outlet />
      </Box>
    </Box>
  );
}

export default HomeLayout;