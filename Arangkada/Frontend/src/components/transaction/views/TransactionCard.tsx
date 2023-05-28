import { Transaction } from '../../../services/dataTypes';
import PaymentIcon from '@mui/icons-material/Payment';
import RentIcon from '@mui/icons-material/CarRental';
import { Link } from "react-router-dom";
import { Button, Card, CardActions, CardHeader, Divider, Stack, Typography } from "@mui/material";

type TransactionCardProps = {
    transactions: Transaction,
  }

const TransactionCard = ({ transactions }: TransactionCardProps) => {
    return (
        <>
        <Card>
          <CardHeader 
            title={transactions.driverName}
            subheader={
                <>
                  <Stack spacing={0.5} direction="row" alignItems="center">
                    <PaymentIcon /> <Typography variant="body1">PHP {transactions.amount}.00</Typography>
                  </Stack>
                  <Stack spacing={0.5} direction="row" alignItems="center">
                    <RentIcon /> <Typography variant="body1">Date Paid: {transactions.date}</Typography>
                  </Stack>
                  <Typography
                    variant="body1">Transaction Id: <b>{transactions.id}</b>
                  </Typography>
                </>
                }
          />
          <Divider />
          <CardActions>
          <Stack direction={{ xs: "column-reverse", md: "row" }} width="100%" spacing={{ xs: 2, md: 3 }} justifyContent="end" padding={1}>
            <Link to={transactions.id + "/delete"} style={{ textDecoration: 'none' }}>
              <Button
                size="small"
                variant="contained"
                className='remove'
                color="error"
                sx={{ width: "150px" }}>
                Delete
              </Button>
            </Link>
            <Link to={transactions.id + "/edit"} style={{ textDecoration: 'none' }}>
              <Button
                size="small"
                variant="contained"
                sx={{ width: "150px" }}>
                Edit
              </Button>
            </Link>
          </Stack>
        </CardActions>         
        </Card>
        </>
      );
}

export default TransactionCard;