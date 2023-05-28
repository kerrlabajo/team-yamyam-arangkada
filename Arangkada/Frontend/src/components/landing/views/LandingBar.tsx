import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { ButtonGroup, ListItem, ListItemButton, Stack } from '@mui/material';
import { NavLink } from 'react-router-dom';
import LogoBrown from '../../../images/logobrown.png';
import LogoWhite from '../../../images/logowhite.png';

type Props = {
  colorScheme: 'brown' | 'white';
};

const LandingAppBar: React.FC<Props> = ({ colorScheme }) => {
  const navItems: { text: string; link: string }[] = [
    { text: 'Contact Us', link: '/contact' },
    { text: 'Login', link: '/' },
    { text: 'Register', link: '/registration' },
  ];

  const logo = colorScheme === 'brown' ? LogoBrown : LogoWhite;
  const stackColor = colorScheme === 'brown' ? '#90794C' : '#ffffff';
  const buttonColor = colorScheme === 'brown' ? '#90794C' : '#ffffff';
  const backgroundColor = colorScheme === 'brown' ? '#ffffff' : '#D2A857';

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar component="nav" sx={{ backgroundColor, height: 64 }}>
        <Toolbar>
          <Typography
            variant="h5"
            component="div"
            sx={{ flexGrow: 1, display: { xs: 'none', sm: 'block' } }}
          >
            <Stack direction="row">
              <img
                src={logo}
                alt="arangkada logo"
                style={{ width: 40, height: 40, paddingTop: 18, paddingLeft: 30 }}
              />
              <strong>
                <p style={{ paddingLeft: 10, color: stackColor }}>Arangkada</p>
              </strong>
            </Stack>
          </Typography>
          <Box sx={{ display: { xs: 'none', sm: 'block' } }}>
            {navItems.map((listItem, index) => (
              <ButtonGroup orientation="horizontal" key={index}>
                <ListItem disableGutters>
                  <ListItemButton
                    sx={{ color: buttonColor }}
                    component={NavLink}
                    to={listItem.link}
                  >
                    {listItem.text}
                  </ListItemButton>
                </ListItem>
              </ButtonGroup>
            ))}
          </Box>
        </Toolbar>
      </AppBar>
    </Box>
  );
};

export default LandingAppBar;
