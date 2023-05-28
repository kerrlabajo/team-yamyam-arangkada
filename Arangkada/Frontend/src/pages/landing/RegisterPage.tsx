import RegisterForm from "../../components/landing/actions/RegisterForm"
import Welcome from "../../components/landing/views/Welcome"
import Footer from "../../components/shared/Footer"
import LandingAppBar from "../../components/landing/views/LandingBar"

export default function RegisterPage() {
    return (
      <div>
        <LandingAppBar colorScheme="brown"/>
        <div className="App">
            <div className="bstyle">
              <div className='contain'>
                <div className='wrapper'>
                  <div className='left'>
                    <Welcome line1='Welcome to' line2='Arangkada' line3='We get you moving!' isLogin={false}></Welcome>
                  </div>
                  <div className='right'>
                    <RegisterForm />
                  </div>
                </div>
              </div>
              <Footer name="Adrian Jay Barcenilla" course="BSCS" section="F1"/>
            </div>
        </div>
      </div>
      
    )
}