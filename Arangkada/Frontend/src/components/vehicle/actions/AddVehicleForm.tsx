import { ChangeEvent, useContext, useState, useEffect } from "react";
import { Button, Grid, Stack, TextField, } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { vehicleService } from "../../../services/vehicleService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

const AddVehicleForm = () => {
    const { user } = useContext(UserContext) as UserContextType;
    const navigate = useNavigate();
    const { setMessage } = useContext(MessageContext) as MessageContextType;

    useEffect(() => {
        if(user === null) {
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate("/", { replace: true })
        }
    }, [user, navigate, setMessage])

    const [data, setData] = useState({
        operatorName: "",
        crNumber: "",
        plateNumber: "",
        bodyType: "",
        make: "",
        distinctionLabel: "",
        rentFee: 0,
        rentStatus: false
    })

    const onSubmit = async (event: { preventDefault: () => void; }) => {
        event.preventDefault();

        if(user !== null) {
            vehicleService.addVehicle({
                operatorName: user.fullName,
                crNumber: data.crNumber,
                plateNumber: data.plateNumber,
                bodyType: data.bodyType,
                make: data.make,
                distinctionLabel: data.distinctionLabel,
                rentFee: data.rentFee,
                rentStatus: data.rentStatus
            })
            .then(() => {
                setMessage("Vehicle Successfully Added.");
                navigate('/vehicles')
            })
            .catch((error) => {
                setMessage(error.response.data || "Failed to add vehicle.");
            })
        }
        else{
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate("/", { replace: true })
        }
    }
    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setData({ ...data, [e.target.name]: e.target.value });
    };
    return (
        <Grid container spacing={4} onSubmit={onSubmit} component="form">
           
            <Grid item xs={12} md={4}>
                <TextField
                    onChange={handleChange}
                    value={data.plateNumber}
                    name="plateNumber"
                    label="Plate Number"
                    size="small"
                    fullWidth
                    required
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={4}>
                <TextField
                    onChange={handleChange}
                    value={data.crNumber}
                    name="crNumber"
                    label="CR Number"
                    size="small"
                    fullWidth
                    required
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={4}>
                <TextField
                    onChange={handleChange}
                    value={data.bodyType}
                    name="bodyType"
                    label="Body Type"
                    size="small"
                    required
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={4}>
                <TextField
                    onChange={handleChange}
                    value={data.make} 
                    label="Make"
                    size="small"
                    variant="outlined"
                    name="make"
                    required
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={4}>
                <TextField
                    onChange={handleChange}
                    value={data.distinctionLabel} 
                    label="Distinction Label"
                    size="small"
                    variant="outlined"
                    name="distinctionLabel"
                    required
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField
                    onChange={handleChange}
                    value={data.rentFee} 
                    label="Rent Fee"
                    size="small"
                    variant="outlined"
                    name="rentFee"
                    required
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} >
                <Stack spacing={3} direction={{ xs: "column-reverse", md: "row" }} sx={{ justifyContent: "end" }}>
                    <Button onClick={() => navigate(-1)} color="secondary" variant="contained" sx={{ width: "250px" }}>Cancel</Button>
                    <Button type="submit" variant="contained" sx={{ width: "250px" }}>Add Vehicle</Button>
                </Stack>
            </Grid>
        </Grid>
    );
}
export default AddVehicleForm;