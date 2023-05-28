import { Box } from "@mui/material";
import Footer from "../../components/shared/Footer";
import PageHeader from "../../components/shared/PageHeader";
import AssignDriverForm from "../../components/driver/actions/AssignDriverForm";

const AssignDriverPage = () => {

  return ( 
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Assign Driver"/>
        <br></br>
        <br></br>
        <AssignDriverForm/>
      </Box>
      <Footer name="John William Miones" course="BSCS" section="F1"/>
    </>
   );
}
export default AssignDriverPage;