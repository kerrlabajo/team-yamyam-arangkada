import { Button, Grid, TextField } from "@mui/material"
import { ChangeEvent, useContext, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { operatorService } from "../../../services/operatorService";
import operatorPic from "../../../images/operator.png"
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";
import { UserContext, UserContextType } from "../../../contexts/UserContext";

export default function VerifyEmailForm() {
    const { setMessage } = useContext(MessageContext) as MessageContextType;
    const { handleSetUser } = useContext(UserContext) as UserContextType;
    const navigate = useNavigate();
    const location = useLocation();

    const [data, setData]= useState({
        verificationCode: "",
    })


    const verifyOperator = async (event: { preventDefault: () => void; }) =>{
        event.preventDefault();

        operatorService.getOperatorById (
            location.state.id
        )
        .then(async (res)=> {
            if(await operatorService.getIsVerifiedById(location.state.id) === false){
                if(data.verificationCode === res.verificationCode){
                    operatorService.updateVerification(res.id, true)
                    .then((res) => {
                        setMessage("Email Successfully Verified.")
                        handleSetUser({ // Sets the user in the context
                            id: res.id,
                            fullName: res.fullName,
                        });
                        navigate("/", { replace: true })
                    })
                    .catch((err) => {
                        setMessage(err.response.data || "Failed to verify email.")
                    })
                }
                else{
                    setMessage("Incorrect Verification Code. Please try again.")
                }
                
            }
            else{
                setMessage("Email has already been verified.")
                navigate("/home", { replace: true })
            }

        })
        .catch((err) => setMessage(err.response.data || "Failed to get operator's verification status."))
    }

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setData({ ...data, [e.target.name]: e.target.value });
    };

    return (
        <Grid onSubmit={verifyOperator} component="form">
        <div className="contain2">
           <h3>Email Verification</h3>
           <img src={operatorPic} alt="operator pic" style={{width: 210, height: 210, marginLeft:'150px', marginRight:'150px'}}/>
           <i><p style={{fontSize: '15px'}}>Please enter the code that was sent to your email ${location.state.id}</p></i>
           <TextField 
                required id="outlined-basic" 
                label="Verification Code" 
                name="verificationCode" 
                variant="outlined" 
                value={data.verificationCode} 
                onChange={handleChange} 
                sx={{margin: 1, width: { sm: 400, md: 400 }}}
            />
            <br></br>
           <Button variant="contained" type="submit" style={{backgroundColor: '#D2A857', marginTop: 25, paddingInline: 40, marginBottom: '45px'}}>SUBMIT</Button><br></br>
        </div>
        </Grid>
    )
}