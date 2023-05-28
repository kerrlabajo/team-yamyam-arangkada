import './styles/SupportStyles.css';
import { useContext, useEffect } from 'react';
import { ThemeProvider } from '@emotion/react';
import theme from './styles/theme';
import ModalProvider from 'mui-modal-provider';
import { UserContext, UserContextType } from './contexts/UserContext';
import MessageContextProvider from './contexts/MessageContext';
import Snackbar from './components/shared/Snackbar';
import AppRoutes from './AppRoutes';

const App = () => {
  const { user } = useContext(UserContext) as UserContextType;

  useEffect(() => {
    window.localStorage.setItem("ARANGKADA_USER", JSON.stringify(user));
  }, [user])

  return (
    <ThemeProvider theme={theme}>
      <ModalProvider>
        <MessageContextProvider>
        <AppRoutes />
          <Snackbar />
        </MessageContextProvider>
      </ModalProvider>
    </ThemeProvider>
  );
}

export default App;
