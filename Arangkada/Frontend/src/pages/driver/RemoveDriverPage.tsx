import { Box, Divider } from "@mui/material";
import Footer from "../../components/shared/Footer";
import RemoveDriverForm from "../../components/driver/actions/RemoveDriverForm";
import PageHeader from "../../components/shared/PageHeader";



const RemoveDriverPage = () => {
 
  return ( 
    <Box sx={{ padding: "12px 0 0" }}>
      <PageHeader title="Remove Driver"/>
      <RemoveDriverForm/>
      <Divider/>
      <Footer name="John William Miones" course="BSCS" section="F1"/>
    </Box>
   );
}
 
export default RemoveDriverPage;