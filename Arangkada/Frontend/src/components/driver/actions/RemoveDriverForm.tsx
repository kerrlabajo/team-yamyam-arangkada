import {  useContext, useEffect, useState } from "react";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, Grid, Stack, TextField,IconButton,InputAdornment, } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { operatorService } from "../../../services/operatorService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";
import bcrypt from 'bcryptjs';
import { driverService } from "../../../services/driverService";

const RemoveDriverForm  = () =>{
    const navigate = useNavigate();
    const para = useParams() as { id: string };
    const { user } = useContext(UserContext) as UserContextType;
    const { setMessage } = useContext(MessageContext) as MessageContextType;
    const [password, setPassword] = useState<string>("");
    const [showPassword, setShowPassword] = useState(false);
    const [passwordError, setPasswordError] = useState<string | null>(null);
    const [data, setData]= useState({
        fullName: "",
    })

    useEffect(() => {
        if(user === null){
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
    }, [user, navigate]);

    const removeDriver = async(e: { preventDefault: () => void; })  => {
        e.preventDefault();
        
       if(user !== null){
            operatorService.getPasswordById(user.id).then((encryptedPassword) => {
                if (encryptedPassword !== "") {
                    bcrypt.compare(password, encryptedPassword).then((result) => {
                        if (result) {
                            driverService.removeDriver(para.id).then((res) => {
                                if(res){
                                    setMessage("Driver Successfully Removed.");
                                    navigate("/drivers", { replace: true });
                                }
                                else{
                                    setMessage("Failed to remove driver. Returned false.");
                                }
                            }).catch((error) => {
                                setMessage(error.response.data || "Failed to remove driver.");
                            })
                        }
                        else {
                            setPasswordError("Password is incorrect.");
                        }
                    }).catch((error) => setMessage(error.response.data || "Failed to compare password."));
                } else {
                    setPasswordError("Password is incorrect.");
                }
            }).catch((error) => setMessage(error.response.data || "Failed to retrieve password."));
        }
        else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
       }
    useEffect(() => {
        driverService.getDriverById(para.id).then((response) => {
            setData(response);
        })
        .catch((error) => {
            setMessage("Failed to retrieve driver to remove.");
        })
    }, [para.id, setMessage]);

    const handlePasswordShow = () => {
        setShowPassword(!showPassword);
    }
  return ( 
    <>
    <Grid container spacing={2} onSubmit={removeDriver} component="form" sx={{marginTop: 0, marginBottom: 5}}>
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
            If you choose to continue, driver {data.fullName}, will be removed and will not be working under in your account.
        </p>
    </Grid>
    <Grid item xs={12} md={12} sx={{marginLeft: 21, marginTop: 2}}>
        <h3 style={{fontFamily:"sans-serif"}}>
            Are you sure you want to remove this driver?
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
 
export default RemoveDriverForm;