import { Box } from "@mui/material";
import Footer from "../../components/shared/Footer";
import EditVehicleForm from "../../components/vehicle/actions/EditVehicleForm";
import PageHeader from "../../components/shared/PageHeader";



const EditVehiclePage = () => {

  return ( 
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Update Vehicle Information"/>
        <br></br>
        <br></br>
        <EditVehicleForm/>
      </Box>
      <Footer name="Olivein Kurl Potolin" course="BSCS" section="F1"/>
    </>
   );
}
export default EditVehiclePage;