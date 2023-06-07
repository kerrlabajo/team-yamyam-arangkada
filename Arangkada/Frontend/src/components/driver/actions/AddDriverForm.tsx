import { ChangeEvent, useContext, useState, useEffect } from "react";
import { Button, Grid, Stack, TextField, } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { driverService } from "../../../services/driverService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

const AddDriverForm = () => {
    const { user } = useContext(UserContext) as UserContextType;
    const navigate = useNavigate();
    const { setMessage } = useContext(MessageContext) as MessageContextType;

    useEffect(() => {
        if(user === null) {
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate("/", { replace: true })
        }
    }, [user, navigate])

    const [data, setData] = useState({
        operatorName: "",
        fullName: "",
        address: "",
        contactNumber: "",
        licenseNumber: "",
        expirationDate: "",
        dlCodes: "",
        category: ""
    })

    const onSubmit = async (event: { preventDefault: () => void; }) => {
        event.preventDefault();

        if(user !== null) {
            driverService.addDriver({
                operatorName: user.fullName,
                fullName: data.fullName,
                address: data.address,
                contactNumber: data.contactNumber,
                licenseNumber: data.licenseNumber,
                expirationDate: data.expirationDate,
                dlCodes: data.dlCodes,
                category: data.category
            })
            .then(() => {
                setMessage("Driver Successfully Added.");
                navigate('/drivers')
            }).catch((error) => {
                setMessage(error.response.data || "Failed to add driver.");
            })
        }
        else{
            console.log('No user logged in')
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
           
            <Grid item xs={12} md={6}>
                <TextField
                    onChange={handleChange}
                    value={data.fullName}
                    name="fullName"
                    label="Full Name"
                    size="small"
                    fullWidth
                    required
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField
                    onChange={handleChange}
                    value={data.address}
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
                    value={data.contactNumber}
                    name="contactNumber"
                    label="Contact Number"
                    size="small"
                    required
                    fullWidth
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField
                    onChange={handleChange}
                    value={data.licenseNumber}
                    label="License Number"
                    name="licenseNumber"
                    size="small"
                    variant="outlined"
                    fullWidth
                    required
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField
                    onChange={handleChange}
                    value={data.dlCodes}
                    name="dlCodes"
                    label="DL Codes"
                    size="small"
                    fullWidth
                    required
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField
                    onChange={handleChange}
                    value={data.expirationDate}
                    name="expirationDate"
                    label="Expiration Date"
                    size="small"
                    fullWidth
                    required
                >
                </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
                <TextField
                    onChange={handleChange}
                    value={data.category}
                    name="category"
                    label="Category"
                    size="small"
                    fullWidth
                    required
                >
                </TextField>
            </Grid>
            <Grid item xs={12} >
                <Stack spacing={3} direction={{ xs: "column-reverse", md: "row" }} sx={{ justifyContent: "end" }}>
                    <Button onClick={() => navigate(-1)} color="secondary" variant="contained" sx={{ width: "250px" }}>Cancel</Button>
                    <Button type="submit" variant="contained" sx={{ width: "250px" }}>Add Driver</Button>
                </Stack>
            </Grid>
        </Grid>
    );
}
export default AddDriverForm;