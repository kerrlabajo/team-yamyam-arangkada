import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, TextField, InputAdornment, IconButton, Grid } from "@mui/material"
import { ChangeEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { operatorService } from "../../../services/operatorService";
import { useContext } from "react";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

export default function RegisterForm() {
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();
    const { setMessage } = useContext(MessageContext) as MessageContextType;

    const [data, setData]= useState({
        fullName: "",
        username: "",
        password: "",
        email: ""
    })

    const registerOperator = async (event: { preventDefault: () => void; }) =>{
        event.preventDefault();
        
        operatorService.register(  {
            fullName: data.fullName,
            username: data.username,
            password: data.password,
            email: data.email
        })
        .then((res)=> {
            navigate("/registration/verify", {state: {id: res.id,}
        });} )
        .catch((err) => setMessage(err.response.data || "Failed to register operator."))
        
    }

    const handlePasswordShow = () => {
        setShowPassword(!showPassword);
    }

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setData({ ...data, [e.target.name]: e.target.value });
    };

    const handleLoginClick = () => {
        navigate("/", { replace: true });
    }

    return (
        <Grid onSubmit={registerOperator} component="form">
        <div className="regform">
            <strong><p style={{color: '#646464', textAlign: 'left', lineHeight: '.2em'}}>Registration</p></strong>
            <hr className="line"></hr>

                <TextField id="outlined-basic" required label="Full Name" variant="outlined" value={data.fullName} onChange={handleChange} name="fullName" sx={{margin: 1, width:{sd: 700, md: 700}}}/><br></br>
                <TextField id="outlined-basic" required label="Email" variant="outlined" value={data.email} onChange={handleChange} name="email" sx={{margin: 1, width:{sd: 700, md: 700}}}/><br></br>
                <TextField id="outlined-basic" required label="Username" variant="outlined" value={data.username} onChange={handleChange} name="username" sx={{margin: 1, width:{sd: 700, md: 700}}}/><br></br>
                <TextField 
                    onChange={handleChange}
                    type={showPassword? "text": "password"} 
                    value={data.password} 
                    label="Password"
                    name="password"
                    required
                    sx={{margin: 1, width:{sd: 700, md: 700}}} 
                    InputProps={{ endAdornment: (<InputAdornment position="end"> <IconButton onClick={handlePasswordShow}>{showPassword? <VisibilityOff />: <Visibility /> }</IconButton> </InputAdornment>) }} 
                />
            <Button variant="contained" type="submit" style={{backgroundColor: '#D2A857', marginTop: 25, paddingInline: 40}}>Continue</Button><br></br>
            <p style={{color: 'gray', fontSize: '15px'}}>By continuing, you agree to Arangkadas's <a href="https://www.lipsum.com/"  className="links">Terms of Service</a> and acknowledge you've read our <a href="https://www.lipsum.com/" className="links">Privacy Policy</a></p>

            <Button className="links" onClick={handleLoginClick} style={{fontSize: '18px'}} variant="text">Have an account? Log in</Button>
        
        </div>
        </Grid>
    )
}