import { Box, Divider } from "@mui/material";
import Footer from "../../components/shared/Footer";
import RemoveVehicleForm from "../../components/vehicle/actions/RemoveVehicleForm";
import PageHeader from "../../components/shared/PageHeader";

const RemoveVehiclePage = () => {
 
  return ( 
    <Box sx={{ padding: "12px 0 0" }}>
      <PageHeader title="Delete Vehicle"/>
      <RemoveVehicleForm/>
      <Divider/>
      <Footer name="Olivein Kurl Potolin" course="BSCS" section="F1"/>
    </Box>
   );
}
 
export default RemoveVehiclePage;