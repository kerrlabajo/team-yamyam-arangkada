import { Box } from "@mui/material";
import Instructions from "../../components/shared/Instructions";
import Footer from "../../components/shared/Footer";
import AddDriverForm from "../../components/driver/actions/AddDriverForm";
import PageHeader from "../../components/shared/PageHeader";

const AddDriverPage = () => {
 
  return ( 
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Add Driver"/>
        <br></br>
        <Instructions
          header="Please provide driver details."
          subheader="The spelling and other information should be entered accurately, and duplicate drivers are not allowed."
        />
        <br></br>
        <br></br>
        <AddDriverForm/>
      </Box>
      <Footer name="John William Miones" course="BSCS" section="F1"/>
    </>
   );
}
export default AddDriverPage;