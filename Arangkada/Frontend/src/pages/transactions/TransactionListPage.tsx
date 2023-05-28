import { Box, Typography } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import Footer from "../../components/shared/Footer";
import PageHeader from "../../components/shared/PageHeader";
import SearchFilterForm from "../../components/shared/SearchFilterForm";
import { Transaction } from "../../services/dataTypes";
import { transactionService } from "../../services/transactionService";
import { UserContext, UserContextType } from "../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../contexts/MessageContext";
import { useNavigate } from "react-router-dom";
import TransactionCardList from "../../components/transaction/views/TransactionCardList";

const VehicleListPage = () => {
  const { user } = useContext(UserContext) as UserContextType;
  const { setMessage } = useContext(MessageContext) as MessageContextType;
  const navigate = useNavigate();
  const [filteredTransactions, setFilteredTransactions] = useState<Transaction[]>([]);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [data, setData] = useState<Partial<Transaction>>({
    operatorName: "",
    driverName: "",
    amount: 0,
    date: "",
  });

  useEffect(() => {
    if (user !== null) {
      transactionService
        .getTransactionsByOperator(user.id)
        .then((response) => {
          setTransactions(response);
        })
        .catch((error) => {
          setMessage(error.response.data || "Failed to retrieve transactions.")
        });
    }
    else{
      console.log('No user logged in')
      setMessage("No user logged in");
      window.localStorage.removeItem("ARANGKADA_USER");
      navigate('/', { replace: true });
    }
  }, [user, setMessage, navigate]);

  useEffect(() => {
    setFilteredTransactions(transactions);
  }, [transactions]);

  const handleFilterSubmit = (filters: Partial<Transaction>) => {
    const filtered = transactions.filter((transaction) => {
      for (const key in filters) {
        if (
          filters.hasOwnProperty(key) &&
          transaction.hasOwnProperty(key) &&
          filters[key] !== undefined &&
          filters[key] !== null &&
          String(transaction[key]).toLowerCase().includes(
          String(filters[key]).toLowerCase()
          )
        ) {
          return true;
        }
      }
      return false;
    });

    setFilteredTransactions(filtered);
  };

  const handleFilterClear = () => {
    setFilteredTransactions(transactions);
    setData({
      operatorName: "",
      driverName: "",
      amount: 0,
      date: "",
    });
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setData((prevData) => ({ ...prevData, [name]: value }));
  };

  return (
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Transactions" />
        <br />
        <SearchFilterForm
          objectProperties={Object.keys(data) as (keyof Transaction)[]}
          data={data}
          handleInputChange={handleInputChange}
          handleFilterSubmit={handleFilterSubmit}
          handleFilterClear={handleFilterClear}
        />
        <br />
        {filteredTransactions.length !== 0 ? (
          <TransactionCardList transactions={filteredTransactions} />
        ) : (
          <Typography variant="body1" color="text.secondary">
            No transactions added.
          </Typography>
        )}
      </Box>
      <Footer name="Alys Anthea Carillo" course="BSCS" section="F1" />
    </>
  );
};

export default VehicleListPage;