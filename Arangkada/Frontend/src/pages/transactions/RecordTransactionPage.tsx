import { Box } from "@mui/material";
import Instructions from "../../components/shared/Instructions";
import AddTransactionForm from "../../components/transaction/actions/AddTransactionForm";
import PageHeader from "../../components/shared/PageHeader";
import Footer from "../../components/shared/Footer";

const RecordTransactionPage = () => {
    
    return (
      <>
        <Box mt="12px" display="flex" flexDirection="column" sx={{ minHeight: "80vh" }}>
          <PageHeader title="Record Transaction" />
          <br></br>
          <Instructions header="Please select the driver to record their transaction." subheader="" />
          <br></br>
          <br></br>
          <AddTransactionForm />
        </Box>
        <Footer name="Alys Anthea Carillo" course="BSCS" section="F1" />
      </>
    );
  }
  
  export default RecordTransactionPage;