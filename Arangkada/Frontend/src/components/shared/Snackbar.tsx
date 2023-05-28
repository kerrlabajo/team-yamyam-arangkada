import { Snackbar as SnackbarBase, IconButton } from "@mui/material";
import { useContext } from "react";
import { Close } from "@mui/icons-material";
import { MessageContext, MessageContextType } from "../../contexts/MessageContext";

const Snackbar = () => {
  const { message, setMessage } = useContext(MessageContext) as MessageContextType;

  const handleClose = (_event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }

    setMessage(null);
  }

  return (
    <SnackbarBase
      open={message !== null}
      autoHideDuration={3000}
      onClose={handleClose}
      message={message}
      action={
        <IconButton onClick={handleClose} color="inherit">
          <Close fontSize="small" />
        </IconButton>
      }
    />
  );
}

export default Snackbar;