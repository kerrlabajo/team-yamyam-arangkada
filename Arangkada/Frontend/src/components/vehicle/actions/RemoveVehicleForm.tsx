import {  useContext, useEffect, useState } from "react";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, Grid,  Stack, TextField,IconButton,InputAdornment, } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { vehicleService } from "../../../services/vehicleService";
import { operatorService } from "../../../services/operatorService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";
import bcrypt from 'bcryptjs';

const RemoveVehicleForm  = () =>{
    const navigate = useNavigate();
    const para = useParams() as { id: string };
    const { user } = useContext(UserContext) as UserContextType;
    const { setMessage } = useContext(MessageContext) as MessageContextType;
    const [password, setPassword] = useState<string>("");
    const [showPassword, setShowPassword] = useState(false);
    const [passwordError, setPasswordError] = useState<string | null>(null);
    const [data, setData]= useState({
        plateNumber: "",
    })

    useEffect(() => {
        if(user === null) {
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate("/", { replace: true })
        }
    }, [user, navigate])

    const deleteVehicle = async(e: { preventDefault: () => void; })  => {
        e.preventDefault();
        
       if(user !== null){
            operatorService.getPasswordById(user.id)
            .then((encryptedPassword) => {
                if (encryptedPassword === "") {
                    setPasswordError("Password is incorrect.");
                    return;
                }
            
                bcrypt.compare(password, encryptedPassword).then((result) => {
                    if (result) {
                        vehicleService.removeVehicle(para.id)
                        .then((res) => {
                            if(res){
                                setMessage("Vehicle Successfully Removed.");
                                navigate("/vehicles", { replace: true });
                            }
                            else{
                                setMessage("Failed to remove vehicle. Returned false.");
                            }
                        })
                        .catch((error) => {
                            setMessage(error.response.data || "Failed to remove vehicle.");
                        })
                    } else {
                        setPasswordError("Password is incorrect. Please try again.");
                    }
                }).catch((error) => setMessage(error.response.data || "Failed to compare password."));
            })
            .catch((error) => setMessage(error.response.data || "Failed to retrieve password."));
        }
        else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
       }
    useEffect(() => {
        vehicleService.getVehicleById(para.id).then((response) => {
            setData(response);
        })
        .catch((error) => {
            setMessage(error.response.data || "Failed to retrieve vehicle.");
        })
    }, [para.id, setMessage]);

    const handlePasswordShow = () => {
        setShowPassword(!showPassword);
    }
  return ( 
    <>
    <Grid container spacing={2} onSubmit={deleteVehicle} component="form" sx={{marginTop: 0, marginBottom: 5}}>
    <Grid item xs={12} md={6} sx={{marginLeft: 20, marginTop: 3}}>
        <h2 style={{fontFamily:"sans-serif"}}> Re-enter your password: </h2>
        <TextField 
                onChange={(event) => setPassword(event.target.value)}
                value={password} 
                type={showPassword ? "text" : "password"}
                fullWidth
                error={passwordError !== null}
                helperText={passwordError}
                name="password"
                id="filled-password-input"
                label="Password"
                required
                InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton onClick={handlePasswordShow}>
                          {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                      </InputAdornment>
                    )
                  }} 
                autoComplete="current-password"
                variant="outlined"
                sx={{margin: 1, marginBottom: 3}}>
        </TextField> 
    </Grid>
    <Grid item xs={12} md={6} sx={{marginLeft: 21, marginTop: 2}}>
        <p style={{fontFamily:"sans-serif"}}> 
            If you choose to continue, this vehicle plate number {data.plateNumber}, will be removed and will not be visible in your account.
        </p>
    </Grid>
    <Grid item xs={12} md={12} sx={{marginLeft: 21, marginTop: 2}}>
        <h3 style={{fontFamily:"sans-serif"}}>
            Are you sure you want to removed this vehicle?
        </h3>
    </Grid>

        <Stack spacing={3} direction={{ xs: "column-reverse", md: "row" }} sx={{ marginTop:2, justifyContent: "end", marginLeft: "180px" }}>
          <Button type="submit" color="error" variant="contained" sx={{ width: "250px" }}>Delete</Button>
          <Button  onClick={() => navigate(-1)} color="primary"variant="contained" sx={{ width: "250px"}}>Cancel</Button>
        </Stack>
        
    </Grid>
    </>
    );
}
 
export default RemoveVehicleForm;