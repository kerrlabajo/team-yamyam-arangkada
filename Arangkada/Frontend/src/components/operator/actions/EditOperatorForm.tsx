import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, Grid, IconButton, InputAdornment, Stack, TextField } from "@mui/material";
import { ChangeEvent, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { operatorService } from "../../../services/operatorService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

export default function EditOperatorForm() {
    
    const navigate = useNavigate();
    const [showPassword, setShowPassword] = useState(false);
    const { user } = useContext(UserContext) as UserContextType;
    const { setMessage } = useContext(MessageContext) as MessageContextType;

    useEffect(() => {
        if(user === null){
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
    }, [user, navigate]);

    const handlePasswordShow = () => {
        setShowPassword(!showPassword);
    }

    const handleCancelClick = () => {
        navigate(-1);
    }

    const [data, setData]= useState({
        fullName: "",
        username: "",
        password: "",
        email: "",
        verificationStatus: false,
    })

    const { fullName, username, password, email } = data;

    useEffect(() => {
        if(user != null){
            operatorService.getOperatorById(user.id).then((response) => {
                setData(response);
            }).catch((error) => {
                setMessage(error.response.data || "Failed to retrieve operator.");
            })
        }
        else{
            console.log("No user logged in");
            setMessage("No user logged in.");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
      }, [user, setMessage, navigate]);

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setData({ ...data, [e.target.name]: e.target.value });
    };

    const updateOperator = async (e: { preventDefault: () => void; }) => {
        e.preventDefault();
        if(user !== null) {
            operatorService.editOperator(user.id, data).then(() => {
                setMessage("Account updated successfully.");
                navigate(-1);
            }).catch((error) => {
                setMessage(error.response.data || "Failed to update operator.");
            })
        }
        else{
            console.log("No user logged in");
            setMessage("No user logged in.");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
    };
    
    return (
        <Grid component="form" onSubmit={updateOperator}>
            <div>
                <h2 style={{textAlign: 'left', color: '#90794C'}}>User Information</h2>
                <hr className="line"></hr><br></br>
            </div>
            <div className="one">
                <TextField id="outlined" name="fullName" onChange={handleChange} value={fullName} label="Full Name" required sx={{margin: 2}}/>
            </div>
            <div className="two">
                <TextField id="outlined" name="username" onChange={handleChange} value={username}  label="Username" required sx={{margin: 2}}/>
                <TextField id="outlined" name="email" onChange={handleChange} value={email} label="Email" required sx={{margin: 2}}/>
            </div>
            <div className="one">
                <TextField required id="outlined-required" type={showPassword? "text": "password"} 
                label="Password" name="password" value={password} 
                InputProps={
                    { 
                        endAdornment: (
                            <InputAdornment position="end"> 
                                <IconButton onClick={handlePasswordShow}>
                                    {showPassword? <VisibilityOff />: <Visibility />}
                                </IconButton> 
                            </InputAdornment>
                        )
                    }
                } onChange={handleChange} sx={{margin: 2}
            }/>
            </div>
            <Stack direction="row" justifyContent="end" padding={3}>
                <Button variant="contained" onClick={handleCancelClick} style={{backgroundColor: '#828E99', marginTop: 25, paddingInline: 40}}>Cancel</Button>
                <Button variant="contained" type="submit" style={{backgroundColor: '#D2A857', marginTop: 25, paddingInline: 20, marginLeft: 15}}>Save Changes</Button>
            </Stack>
            
        </Grid>
          
            
    )
}