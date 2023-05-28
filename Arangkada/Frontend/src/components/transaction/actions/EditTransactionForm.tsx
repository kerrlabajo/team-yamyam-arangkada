import { ChangeEvent, useContext, useState, useEffect } from "react";
import { Button, Grid, Stack, TextField } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { transactionService } from "../../../services/transactionService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

const EditTransactionForm = () => {
  const { user } = useContext(UserContext) as UserContextType;
  const navigate = useNavigate();
  const para = useParams() as { id: string };
  const { setMessage } = useContext(MessageContext) as MessageContextType;
  const [data, setData] = useState({
    driverName: "",
    amount: 0,
    date: ""
  });  

  useEffect(() => {
    if (user !== null) {
      transactionService.getTransactionById(para.id).then((response) => {
        setData(response);
      }).catch((error) => {
        setMessage(error.response.data || "Failed to get transaction details.");
      });
    }
    else {
      window.localStorage.removeItem("ARANGKADA_USER");
      navigate("/", { replace: true });
    }
  }, [para.id, user, navigate, setMessage]);

  const onSubmit = async (event: { preventDefault: () => void; }) => {
    event.preventDefault();

    if (user !== null) {
      transactionService.editTransaction(para.id, data).then(() => {
        setMessage("Transaction successfully edited.");
        navigate(-1);
      }).catch((error) => {
        setMessage(error.response.data || "Failed to update transaction.");
      });
    }
    else {
      console.log('No user logged in');
      setMessage("No user logged in");
      window.localStorage.removeItem("ARANGKADA_USER");
      navigate("/", { replace: true });
    }
  };

  const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setData({ ...data, [e.target.name]: e.target.value });
  };

  return (
    <Grid container spacing={4} onSubmit={onSubmit} component="form">
      <Grid item xs={12} md={6}>
        <TextField
          onChange={handleChange}
          value={data.driverName}
          label="Driver Name"
          size="small"
          variant="outlined"
          name="driverName"
          disabled
          fullWidth
        />
      </Grid>
      <Grid item xs={12} md={6}>
        <TextField
          onChange={handleChange}
          value={data.amount}
          label="Amount"
          size="small"
          variant="outlined"
          name="amount"
          required
          fullWidth
        />
      </Grid>
      <Grid item xs={12} md={6}>
        <TextField
          onChange={handleChange}
          value={data.date}
          label="Date Paid"
          size="small"
          variant="outlined"
          name="date"
          required
          fullWidth
        />
      </Grid>
      <Grid item xs={12}>
        <Stack spacing={3} direction={{ xs: "column-reverse", md: "row" }} sx={{ justifyContent: "end" }}>
          <Button onClick={() => navigate(-1)} color="secondary" variant="contained" sx={{ width: "250px" }}>Cancel</Button>
          <Button type="submit" variant="contained" sx={{ width: "250px" }}>Save Changes</Button>
        </Stack>
      </Grid>
    </Grid>
  );
};

export default EditTransactionForm;