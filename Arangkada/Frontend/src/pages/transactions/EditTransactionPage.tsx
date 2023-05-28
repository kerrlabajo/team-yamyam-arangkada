import { Box } from "@mui/material";
import PageHeader from "../../components/shared/PageHeader";
import Footer from "../../components/shared/Footer";
import EditTransactionForm from "../../components/transaction/actions/EditTransactionForm";

const EditTransactionPage = () => {

    return ( 
      <>
        <Box mt="12px" sx={{ minHeight: "80vh" }}>
          <PageHeader title="Edit Transaction Information"/>
          <br></br>
          <br></br>
          <EditTransactionForm/>
        </Box>
        <Footer name="Alys Anthea Carillo" course="BSCS" section="F1"/>
      </>
     );
  }
  export default EditTransactionPage;