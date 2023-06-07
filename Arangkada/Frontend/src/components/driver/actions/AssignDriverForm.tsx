import { useContext, useEffect, useState } from "react";
import { Button, FormControl, Grid,  InputLabel, MenuItem, Select, SelectChangeEvent, Stack, TextField, } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { driverService } from "../../../services/driverService";
import { vehicleService } from "../../../services/vehicleService";
import { Vehicle } from "../../../services/dataTypes";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

const AssignDriverForm  = () =>{
    const { user } = useContext(UserContext) as UserContextType;
    const navigate = useNavigate();
    const para = useParams() as { id: string };
    const { setMessage } = useContext(MessageContext) as MessageContextType;
    const [vehicles, setVehicles] = useState<Vehicle[]>([]);

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
        dlCodes: "",
        category: "",
        vehicleAssigned: ""
    })
    const { fullName, address, contactNumber, licenseNumber, expirationDate, dlCodes, vehicleAssigned } = data;
    const assignDriver = async (e: { preventDefault: () => void; }) => {
        e.preventDefault();
        if(user !== null) {
            driverService.assignDriver(para.id, vehicleAssigned).then(() => {
                setMessage("Driver Successfully Assigned to " + vehicleAssigned);
                navigate("/drivers", { replace: true })
                }).catch((error) => {
                    setMessage(error.response.data || "Failed to assign driver to " + vehicleAssigned);
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

            vehicleService.getVehiclesByOperator(user.id).then((response) => {
                setVehicles(response.filter((vehicle) => vehicle.rentStatus === false));
            }).catch((error) => {
                setMessage(error.response.data || "Failed to operator vehicles.");
            })
                
        }
        else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true })
        }
    }, [para.id, user, navigate, setMessage]);
    
    const handleSelectChange = (event: SelectChangeEvent) => {
        setData({ ...data, [event.target.name]: event.target.value });
    }
    return ( 
        <>
        <Grid container spacing={4} onSubmit={assignDriver} component="form">
        <Grid item xs={12} md={6}>
           <TextField 
                disabled
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
                value={address} 
                name="address"
                label="Address" 
                size="small" 
                fullWidth 
                disabled
            > 
            </TextField>
        </Grid>
        <Grid item xs={12} md={6}>
           <TextField 
                value={contactNumber}
                name="contactNumber"
                disabled
                label="Contact Number" 
                size="small" 
                fullWidth 
            >
           </TextField>
        </Grid>
        <Grid item xs={12} md={6}>
            <TextField 
                value={licenseNumber}
                label="License Number" 
                name="licenseNumber"
                disabled
                size="small" 
                variant="outlined" 
                fullWidth
            >
            </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField 
                    value={expirationDate}
                    label="Expiration Date" 
                    size="small" 
                    variant="outlined" 
                    disabled
                    name="expirationDate"
                    fullWidth
                >
                </TextField>
                </Grid>
                <Grid item xs={12} md={6}>
                <TextField 
                    value={dlCodes}
                    label="DL Codes" 
                    size="small" 
                    variant="outlined"
                    name="dlCodes"
                    disabled 
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField 
                    value={data.category}
                    label="Category" 
                    size="small" 
                    variant="outlined"
                    name="category"
                    disabled 
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <FormControl fullWidth size="small">
                <InputLabel>Vehicle Assigned</InputLabel>
                <Select
                    value={data.vehicleAssigned}
                    label="Vehicle Assigned"
                    required
                    name="vehicleAssigned"
                    size="small"
                    onChange={handleSelectChange}
                >
                    {/* Dynamic List of Driver that can be selected */}
                    {vehicles.map((vehicle) => (
                    <MenuItem key={vehicle.id} value={vehicle.plateNumber}>
                        {vehicle.plateNumber}
                    </MenuItem>
                    ))}
                </Select>
                </FormControl>
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
export default AssignDriverForm;