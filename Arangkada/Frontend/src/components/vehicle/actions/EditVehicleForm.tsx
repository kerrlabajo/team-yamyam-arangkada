import { ChangeEvent, useContext, useEffect, useState } from "react";
import { Button, FormControl, Grid,  InputLabel, MenuItem, Select, SelectChangeEvent, Stack, TextField, } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { vehicleService } from "../../../services/vehicleService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

const EditVehicleForm  = () =>{
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
        crNumber: "",
        plateNumber: "",
        bodyType: "",
        make: "",
        distinctionLabel: "",
        rentFee: 0,
        rentStatus: false
    })

    useEffect(() => {
        vehicleService.getVehicleById(para.id).then((response) => {
        setData(response);
        }).catch((error) => {
        console.log(error);
        })
    }, [para.id]);

    const { crNumber, plateNumber, bodyType, make, distinctionLabel, rentFee, rentStatus } = data;
    const updateVehicle = async (e: { preventDefault: () => void; }) => {
        e.preventDefault();

        if(user !== null) {
            if(data.rentFee !== 0){
                if(data.rentStatus !== false){
                    vehicleService.editRentFee(para.id, data.rentFee)
                    .catch((error) => {
                        setMessage(error.response.data || "Failed to update rent fee.");
                        return;
                    })

                    vehicleService.editRentStatus(para.id, data.rentStatus)
                    .catch((error) => {
                        setMessage(error.response.data || "Failed to update rent status.");
                        return;
                    })
                }
                else{
                    vehicleService.editRentFee(para.id, data.rentFee)
                    .catch((error) => {
                        setMessage(error.response.data || "Failed to update rent fee.");
                    })
                }
            }
            else{
                vehicleService.editRentStatus(para.id, data.rentStatus)
                .catch((error) => {
                    setMessage(error.response.data || "Failed to update rent status.");
                })
            }
            setMessage("Vehicle Successfully Updated.");
            navigate('/vehicles')
        }
        else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate("/", { replace: true })
        }
    };
     
    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setData({ ...data, [e.target.name]: e.target.value });
    };
    const handleSelectChange = (event: SelectChangeEvent) => {
        setData({ ...data, [event.target.name]: event.target.value });
    }
    return ( 
        <>
        <Grid container spacing={4} onSubmit={updateVehicle}component="form">
        <Grid item xs={12} md={4}>
           <TextField 
                onChange={handleChange} 
                disabled
                value={plateNumber} 
                name="plateNumber"
                label="Plate Number" 
                size="small"
                fullWidth 
            >
            </TextField> 
        </Grid>
        <Grid item xs={12} md={4}>
           <TextField 
                onChange={handleChange} 
                disabled
                value={bodyType} 
                name="bodyType"
                label="Body Type" 
                size="small" 
                fullWidth 
            > 
            </TextField>
        </Grid>
        <Grid item xs={12} md={4}>
           <TextField 
                onChange={handleChange} 
                value={make}
                name="make"
                disabled
                label="Make" 
                size="small" 
                fullWidth 
            >
           </TextField>
        </Grid>
        <Grid item xs={12} md={6}>
            <TextField 
                onChange={handleChange} 
                value={distinctionLabel}
                label="Distinction Label" 
                name="distinctionLabel"
                disabled
                size="small" 
                variant="outlined" 
                fullWidth
            >
            </TextField>
            </Grid>
        <Grid item xs={12} md={6}>
            <TextField 
                onChange={handleChange} 
                value={crNumber}
                label="Certificate of Registration Number" 
                name="crNumber"
                disabled
                size="small" 
                variant="outlined" 
                fullWidth
            >
            </TextField>
        </Grid>
        <Grid item xs={12} md={6}>
          <FormControl fullWidth size="small">
                <InputLabel >Rent Status</InputLabel>
                    <Select
                        value={rentStatus.toString()}
                        label="Rent Status"
                        required
                        name="rentStatus"
                        size="small" 
                        onChange={handleSelectChange}
                    >
                        <MenuItem value={true.toString()}>Rented</MenuItem>
                        <MenuItem value={false.toString()}>Not Rented</MenuItem>
                    </Select>  
            </FormControl> 
        </Grid>
        <Grid item xs={12} md={6}>
            <TextField 
                onChange={handleChange} 
                value={rentFee}
                label="Rent Fee" 
                name="rentFee"
                required
                size="small" 
                variant="outlined" 
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
export default EditVehicleForm;