import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, TextField, Stack, Paper, InputAdornment, IconButton } from "@mui/material";
import { useContext, useState } from "react"
import { Link, useNavigate } from "react-router-dom";
import { operatorService } from "../../../services/operatorService";
import { MessageContext, MessageContextType } from "../../../contexts/MessageContext";
import { UserContext, UserContextType } from "../../../contexts/UserContext";
import bcrypt from 'bcryptjs';

export default function LoginForm() {
    const navigate = useNavigate();
    const [showPassword, setShowPassword] = useState(false);
    const { handleSetUser } = useContext(UserContext) as UserContextType;
    const { setMessage } = useContext(MessageContext) as MessageContextType;
    const [usernameError, setUsernameError] = useState<string | null>(null);
    const [passwordError, setPasswordError] = useState<string | null>(null);

    window.localStorage.removeItem("ARANGKADA_USER");

    const [loginData, setLoginData] = useState({
        username: "",
        password: ""
    });

    const { username, password } = loginData;

    const onInputChange = (e: any) => {
        setLoginData({ ...loginData, [e.target.name]: e.target.value });
    };

    const handlePasswordShow = () => {
        setShowPassword(!showPassword);
    }

    const checkInfo = async (event: { preventDefault: () => void; }) => {
        event.preventDefault();
        setUsernameError(null);
        setPasswordError(null);

        operatorService.getOperatorByUserName(username).then((operator) => {
          operatorService.getPasswordById(operator.id).then((encryptedPassword) => {
            bcrypt.compare(password, encryptedPassword).then((result) => {
                if (result) {
                    handleSetUser(operator);
                    window.localStorage.setItem("ARANGKADA_USER", JSON.stringify(operator));
                    navigate("/home", { replace: true });
                } else {
                    setPasswordError("Password is incorrect. Please try again.");
                }
                }).catch((error) => setPasswordError(error.response.data || "Failed to compare password."));
          }).catch(() => setMessage("Failed to retrieve password."));
          }).catch(() => setUsernameError("Username does not exist."));
    }

    return (
        <Stack width="100%" alignItems="center" spacing={2}>
            <Paper sx={{ width: "70%", borderRadius: "20px", height: "450px" }}>
                <Stack onSubmit={checkInfo} component="form" padding="64px 74px" spacing={4}>
                    <h1 style={{ color: "#646464", margin: 0 }}>Log in</h1>
                    <TextField
                        size="small"
                        required name="username"
                        label="Username"
                        variant="outlined"
                        value={username}
                        onChange={(e) => onInputChange(e)}
                        error={usernameError !== null}
                        helperText={usernameError}
                    />
                    <TextField
                        required
                        name="password"
                        onChange={(e) => onInputChange(e)}
                        type={showPassword ? "text" : "password"}
                        value={password}
                        label="Password"
                        size="small"
                        fullWidth
                        error={passwordError !== null}
                        helperText={passwordError}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton onClick={handlePasswordShow}>
                                        {showPassword ? <VisibilityOff /> : <Visibility />}
                                    </IconButton>
                                </InputAdornment>
                            )
                        }}
                    />
                    <p style={{ color: 'brown', fontSize: '15px', textAlign: 'center' }}>  <a href="/to-be-implemented" className="links">Forgot password?</a></p>
                    <Stack alignItems="center">
                        <Button variant="contained" type="submit" sx={{ borderRadius: "20px", width: "200px" }}>Log in</Button>
                    </Stack>
                </Stack>
            </Paper>
            <p style={{ color: 'white', fontSize: '16px' }}>Not registered yet?  <Link to="/registration" style={{ color: "white" }}>Create an operator</Link></p>
        </Stack>
    )
}
