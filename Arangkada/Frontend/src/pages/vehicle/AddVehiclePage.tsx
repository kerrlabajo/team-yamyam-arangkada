import { Box } from "@mui/material";
import Instructions from "../../components/shared/Instructions";
import Footer from "../../components/shared/Footer";
import PageHeader from "../../components/shared/PageHeader";
import AddVehicleForm from "../../components/vehicle/actions/AddVehicleForm";

const AddVehiclePage = () => {
 
  return ( 
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Add Vehicle"/>
        <br></br>
        <Instructions
          header="Please provide vehicle details."
          subheader="The spelling and other information should be entered accurately, and duplicate vehicles are not allowed."
        />
        <br></br>
        <br></br>
        <AddVehicleForm/>
      </Box>
      <Footer name="Olivein Kurl Potolin" course="BSCS" section="F1"/>
    </>
   );
}
export default AddVehiclePage;