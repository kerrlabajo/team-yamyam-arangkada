import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, IconButton, InputAdornment, Stack, TextField } from "@mui/material";
import { ChangeEvent, useContext, useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { operatorService } from "../../../services/operatorService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";
import bcrypt from 'bcryptjs';

export default function RemoveOperatorForm() {

    const [showPassword, setShowPassword] = useState(false);
    const [passwordError, setPasswordError] = useState<string | null>(null);
    const navigate = useNavigate();
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

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setData({ ...data, [e.target.name]: e.target.value });
    };

    const [data, setData]= useState({
        password:"",
     })
     const {password} = data;

     const deleteAccount = async(e: { preventDefault: () => void; })  => {
        e.preventDefault();
        if(user !== null){
            operatorService.getPasswordById(user.id).then((encryptedPassword) => {
                if (encryptedPassword !== "") {
                    bcrypt.compare(password, encryptedPassword).then((result) => {
                        if (result) {
                            operatorService.removeOperator(user.id).then((res) => {
                                if(res){
                                    setMessage("Operator Successfully Removed.");
                                    navigate("/", { replace: true });
                                }
                                else{
                                    setMessage("Failed to remove operator. Returned false.");
                                }
                            }).catch((error) => {
                                setMessage(error.response.data || "Failed to remove operator.");
                            })
                        }
                        else {
                            setPasswordError("Password is incorrect.");
                        }
                    }).catch((error) => setMessage(error.response.data || "Failed to compare password."));
                } else {
                    setMessage("Failed to retrieve password. Please try again.");
                }
            }).catch((error) => setMessage(error.response.data || "Failed to retrieve password."));
        }else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
        
    };

    return (
        
            <Stack component="form" onSubmit={deleteAccount}>
            <h1 style={{textAlign: 'left', color: '#90794C'}}>Delete your account</h1>
            <hr className="line"></hr>
            <p style={{textAlign: 'left', fontSize: '20px', paddingTop: 40}}>We're sorry to hear you'd like to delete your account. </p>
            <h5 style={{textAlign: 'left', color: '#90794C', fontSize: 20}}>Re-enter your password:</h5>

            <Stack direction="row" justifyContent="start">
                <TextField 
                    onChange={handleChange}
                    type={showPassword? "text": "password"} 
                    value={password} 
                    label="Password" 
                    name="password"
                    id="filled-password-input"
                    required
                    autoComplete="current-password"
                    variant="outlined"
                    error={passwordError !== null}
                    helperText={passwordError}
                    sx={{margin: 1, width: {sm: 300, md: 300}}}
                    InputProps={{ endAdornment: (<InputAdornment position="end"> <IconButton onClick={handlePasswordShow}>{showPassword? <VisibilityOff />: <Visibility />}</IconButton> </InputAdornment>) }} 
                />
            </Stack>
            
            <p style={{textAlign: 'left', fontSize: '20px'}}>If you choose to continue, your vehicles, drivers, and transactions list will be <br></br> permanently deleted. You won't be visible on Arangkada between now and then.  </p>
            <p style={{textAlign: 'left', fontSize: '20px', paddingTop: 50}}>Do you still want to delete your account?</p>
            <Stack direction="row" justifyContent="start" paddingBottom={7}>
                <Button variant="contained" type="submit" style={{backgroundColor: '#D76666', marginTop: 25, paddingInline: 40}}>Delete</Button>
                <Button variant="contained" onClick={handleCancelClick} style={{backgroundColor: '#D2A857', marginTop: 25, paddingInline: 40, marginLeft: 15}}>Cancel</Button>
            </Stack>
            </Stack>
            
       
    )
}