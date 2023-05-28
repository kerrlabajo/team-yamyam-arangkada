import { Stack } from "@mui/material"
import Welcome from "../../components/landing/views/Welcome"
import Footer from "../../components/shared/Footer"
import LandingAppBar from "../../components/landing/views/LandingBar"
import LoginForm from "../../components/landing/actions/LoginForm"

export default function LoginPage() {
    return (
      <Stack className="bgimg">
        <LandingAppBar colorScheme={"brown"}/>
        <Stack 
          direction={{ xs: "column", md: "row" }} 
          sx={{ margin: "auto", width: "90%", padding: "112px 0 32px 0" }} 
          alignItems="center"
          spacing={{ xs: 4, md: 0 }}
        >
          <Welcome line1='Welcome to' line2='Arangkada' line3='We get you moving!' isLogin={true}/>
          <LoginForm/>
        </Stack> 
        <Footer name="Adrian Jay Barcenilla" course="BSCS" section="F1" />
      </Stack>
    )
}