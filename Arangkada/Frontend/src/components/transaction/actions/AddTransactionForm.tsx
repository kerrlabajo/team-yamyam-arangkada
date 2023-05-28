import { ChangeEvent, useContext, useState, useEffect } from "react";
import { Button, FormControl, Grid, InputLabel, MenuItem, Select, SelectChangeEvent, Stack, TextField } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Driver } from "../../../services/dataTypes";
import { transactionService } from "../../../services/transactionService";
import { driverService } from "../../../services/driverService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

const AddTransactionForm = () => {
  const { user } = useContext(UserContext) as UserContextType;
  const navigate = useNavigate();
  const { setMessage } = useContext(MessageContext) as MessageContextType;

  useEffect(() => {
    if(user === null){
        window.localStorage.removeItem("ARANGKADA_USER");
        navigate('/', { replace: true });
    }
}, [user, navigate]);

  const [data, setData] = useState({
    driverName: "",
    amount: 0,
    date: new Date().toLocaleDateString()
  });  

  const [drivers, setDrivers] = useState<Driver[]>([]); // State to store the list of drivers

  useEffect(() => {
    if (user !== null) {
      driverService.getDriversByOperator(user.id)
        .then((drivers: Driver[]) => {
          setDrivers(drivers.filter((driver) => driver.vehicleAssigned !== null));
        })
        .catch((error) => {
          setMessage(error.response.data || "Failed to get drivers.");
        });
    }
  }, [user, setMessage]);

  const onSubmit = async (event: { preventDefault: () => void; }) => {
    event.preventDefault();

    if (user !== null) {
      transactionService.recordTransaction({
        operatorName: user.fullName,
        driverName: data.driverName,
        amount: data.amount,
        date: data.date
      })
        .then(() => {
          setMessage("Transaction Successfully Recorded.");
          navigate('/transactions', { replace: true });
        })
        .catch((error) => {
          setMessage(error.response.data || "Failed to record transaction.");
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

  const handleSelectChange = (event: SelectChangeEvent) => {
    setData({ ...data, [event.target.name]: event.target.value });
  };

  return (
    <Grid container spacing={4} onSubmit={onSubmit} component="form">
      <Grid item xs={12} md={6}>
        <FormControl fullWidth size="small">
          <InputLabel>Driver Name</InputLabel>
          <Select
            value={data.driverName}
            label="Driver Name"
            required
            name="driverName"
            size="small"
            onChange={handleSelectChange}
          >
            {drivers.map((driver) => (
              <MenuItem key={driver.id} value={driver.fullName}>
                {driver.fullName}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
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
      <Grid item xs={12}>
        <Stack spacing={3} direction={{ xs: "column-reverse", md: "row" }} sx={{ justifyContent: "end" }}>
          <Button onClick={() => navigate(-1)} color="secondary" variant="contained" sx={{ width: "250px" }}>Cancel</Button>
          <Button type="submit" variant="contained" sx={{ width: "250px" }}>Record Transaction</Button>
        </Stack>
      </Grid>
    </Grid>
  );
};

export default AddTransactionForm;