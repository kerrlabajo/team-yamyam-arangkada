import { Box, Divider } from "@mui/material";
import Footer from "../../components/shared/Footer";
import PageHeader from "../../components/shared/PageHeader";
import DeleteTransactionForm from "../../components/transaction/actions/DeleteTransactionForm";

const DeleteTransactionPage = () => {
 
  return ( 
    <Box sx={{ padding: "12px 0 0" }}>
      <PageHeader title="Delete Transaction"/>
      <DeleteTransactionForm/>
      <Divider/>
      <Footer name="Alys Anthea Carillo" course="BSCS" section="F1"/>
    </Box>
   );
}
 
export default DeleteTransactionPage;