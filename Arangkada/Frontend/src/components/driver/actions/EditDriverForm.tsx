import { ChangeEvent, useContext, useEffect, useState } from "react";
import { Button, Grid, Stack, TextField, } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { driverService } from "../../../services/driverService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

const EditDriverForm  = () =>{
    const { user } = useContext(UserContext) as UserContextType;
    const navigate = useNavigate();
    const para = useParams() as { id: string };
    const { setMessage } = useContext(MessageContext) as MessageContextType;
    
    useEffect(() => {
        if(user === null) {
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate("/", { replace: true })
        }
    }, [user, navigate])

    const [data, setData]= useState({
        fullName: "",
        address: "",
        contactNumber: "",
        licenseNumber: "",
        expirationDate: "",
        dlCodes: ""
    })
    const { fullName, address, contactNumber, licenseNumber, expirationDate, dlCodes } = data;
    const updateDriver = async (e: { preventDefault: () => void; }) => {
        e.preventDefault();
        if(user !== null) {
            driverService.editDriver(para.id, data)
            .then(() => {
                setMessage("Driver Successfully Updated.");
                navigate("/drivers", { replace: true })
            })
            .catch((error) => {
                setMessage(error.response.data || "Failed to update driver.");
            })
        }
        else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true })
        }

    };

    useEffect(() => {
        if(user !== null) {
            driverService.getDriverById(para.id).then((response) => {
            setData(response);
            }).catch((error) => {
                setMessage(error.response.data || "Failed to get driver details.");
            })
        }
        else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true })
        }
    }, [para.id, user, navigate, setMessage]);
     
    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setData({ ...data, [e.target.name]: e.target.value });
    };

    return ( 
        <>
        <Grid container spacing={4} onSubmit={updateDriver}component="form">
        <Grid item xs={12} md={6}>
           <TextField 
                onChange={handleChange} 
                required
                value={fullName} 
                name="fullName"
                label="Full Name" 
                size="small"
                fullWidth 
            >
            </TextField> 
        </Grid>
        <Grid item xs={12} md={6}>
           <TextField 
                onChange={handleChange} 
                value={address} 
                name="address"
                label="Address" 
                size="small" 
                fullWidth 
                required
            > 
            </TextField>
        </Grid>
        <Grid item xs={12} md={6}>
           <TextField 
                onChange={handleChange} 
                value={contactNumber}
                name="contactNumber"
                required
                label="Contact Number" 
                size="small" 
                fullWidth 
            >
           </TextField>
        </Grid>
        <Grid item xs={12} md={6}>
            <TextField 
                onChange={handleChange} 
                value={licenseNumber}
                label="License Number" 
                name="licenseNumber"
                required
                size="small" 
                variant="outlined" 
                fullWidth
            >
            </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField 
                    onChange={handleChange} 
                    value={expirationDate}
                    label="Expiration Date" 
                    size="small" 
                    variant="outlined" 
                    required
                    name="expirationDate"
                    fullWidth
                >
                </TextField>
                </Grid>
                <Grid item xs={12} md={6}>
                <TextField 
                    onChange={handleChange} 
                    value={dlCodes}
                    label="DL Codes" 
                    size="small" 
                    variant="outlined"
                    name="dlCodes"
                    required 
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} >
            <Stack spacing={3} direction={{ xs: "column-reverse", md: "row" }} sx={{ justifyContent: "end" }}>
              <Button  onClick={() => navigate(-1)} color="secondary" variant="contained" sx={{ width: "250px" }}>Cancel</Button>
              <Button  type="submit" variant="contained" sx={{ width: "250px"}}>Save Changes</Button> 
            </Stack>
            </Grid>
        </Grid>
        </>
       );
    }
export default EditDriverForm;