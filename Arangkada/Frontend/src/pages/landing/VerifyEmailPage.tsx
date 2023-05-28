import { Box, Toolbar, Typography } from "@mui/material";
import Footer from "../../components/shared/Footer";
import LandingAppBar from "../../components/landing/views/LandingBar";
import EmailVerifyForm from "../../components/landing/actions/VerifyEmailForm";

export default function VerifyEmailPage() {
    return (
        <div>
            <LandingAppBar colorScheme="brown"></LandingAppBar>
            <div className="App">
                <div className="bstyle">
                    <Box component="main" sx={{ p: 6 }}>
                        <Toolbar />
                        <Typography>
                            <EmailVerifyForm />
                        </Typography>
                    </Box>
                    <Footer name="Adrian Jay Barcenilla" course="BSCS" section="F1"/>
                </div>
            </div>
        </div>
    )
}