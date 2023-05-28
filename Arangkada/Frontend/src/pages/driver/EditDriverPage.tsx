import { Box, } from "@mui/material";
import Footer from "../../components/shared/Footer";
import EditDriverForm from "../../components/driver/actions/EditDriverForm";
import PageHeader from "../../components/shared/PageHeader";

const EditDriverPage = () => {

  return ( 
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Update Driver Information"/>
        <br></br>
        <br></br>
        <EditDriverForm/>
      </Box>
      <Footer name="John William Miones" course="BSCS" section="F1"/>
    </>
   );
}
export default EditDriverPage;