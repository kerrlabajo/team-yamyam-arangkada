import { Button, Stack, TextField } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { operatorService } from "../../../services/operatorService";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";

export default function OperatorCard() {
    const navigate = useNavigate();
    const {user} = useContext(UserContext) as UserContextType;
    const { setMessage } = useContext(MessageContext) as MessageContextType;

    const [data, setData]= useState({
        fullName: "",
        username: "",
        email: "",
        isVerified: false,
    })

    const { fullName, username, email, isVerified} = data;
    

    useEffect(() => {
        if(user != null){
            operatorService.getOperatorById(user.id).then((response) => {
                setData(response);
            }).catch((error) => {
                setMessage(error.response.data || "Failed to retrieve operator.")
            })
        }
        else{
            console.log('No user logged in')
            setMessage("No user logged in");
            window.localStorage.removeItem("ARANGKADA_USER");
            navigate('/', { replace: true });
        }
      }, [user, setMessage, navigate]);

    const handleUserEditClick = () => {
        navigate("edit");
       
    }
    const handleDeleteClick = () => {
        navigate("delete");
    }

    return (
        <div>
            <div>
                <h2 style={{textAlign: 'left', color: '#90794C'}}>User Information</h2>
                <hr className="line"></hr><br></br>
            </div>
        
            <div className="one">
                <TextField id="outlined-read-only-input" name="fullName" value={fullName} label="Full Name" InputProps={{readOnly: true,}} sx={{margin: 2}}/>
            </div>
            <div className="three">
                <TextField id="outlined-read-only-input" name="username" value={username}  label="Username" InputProps={{readOnly: true,}} sx={{margin: 2}}/>
                <TextField id="outlined-read-only-input" name="email" value={email} label="Email" InputProps={{readOnly: true,}} sx={{margin: 2}}/>
                <TextField id="outlined-read-only-input" name="isVerified" value={isVerified === true ? "Verified" : "Not Verified"} label="Verification Status" InputProps={{readOnly: true,}} sx={{margin: 2}}/>
            </div>
            
            <Stack direction="row" justifyContent="end" paddingBottom={7}>
                <Button variant="contained" onClick={handleDeleteClick} style={{backgroundColor: '#D76666', marginTop: 25}}>Delete</Button>
                <Button variant="contained" onClick={handleUserEditClick} style={{backgroundColor: '#D2A857', marginTop: 25, marginLeft: 15, marginRight: 15, paddingInline: 60}}>Edit Account</Button>
            </Stack>
        
        </div>
          
            
    )
}