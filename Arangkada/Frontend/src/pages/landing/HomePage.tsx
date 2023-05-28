import { Box, Grid, Paper, Typography } from "@mui/material";
import DashboardCard from "../../components/landing/views/DashboardCard";
import { CarRental, Commute } from "@mui/icons-material";
import PriceCheckIcon from '@mui/icons-material/PriceCheck';
import PageHeader from "../../components/shared/PageHeader";
import Footer from "../../components/shared/Footer";
import { useContext, useEffect, useState } from "react";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { Driver, Vehicle, Transaction } from "../../services/dataTypes";
import Loading from "../../components/shared/Loading";
import ResponseError from "../../components/shared/ResponseError";
import { UserContext, UserContextType } from "../../contexts/UserContext";
import { driverService } from "../../services/driverService";
import { vehicleService } from "../../services/vehicleService";
import { operatorService } from "../../services/operatorService";
import { transactionService } from "../../services/transactionService";
import { MessageContext, MessageContextType } from "../../contexts/MessageContext";
import { useNavigate } from "react-router-dom"; 

const HomePage = () => {
    const { user } = useContext(UserContext) as UserContextType;
    const { setMessage } = useContext(MessageContext) as MessageContextType;
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [drivers, setDrivers] = useState<Driver[]>([]);
    const [vehicles, setVehicles] = useState<Vehicle[]>([]);
    const [transactions, setTransactions] = useState<Transaction[]>([]);
  
    const columns: GridColDef[] = [
      { field: "id", headerName: "ID", flex: 1, minWidth: 100 },
      { field: "driver", headerName: "Driver", flex: 1, minWidth: 100, valueGetter: (param) => param.row.fullName},
      { field: "vehicleAssigned", headerName: "Vechicle Assigned", flex: 1, minWidth: 100, valueGetter: (param) => param.row.vehicleAssigned },
      { field: "address", headerName: "Address", flex: 1, minWidth: 100, valueGetter: (param) => param.row.address },
      { field: "contactNumber", headerName: "Contact Number", flex: 1, minWidth: 100, valueGetter: (param) => param.row.contactNumber },
      { field: "expirationDate", headerName: "Expiration Date", flex: 1, minWidth: 100, valueGetter: (param) => param.row.expirationDate },
    ]
  
    useEffect(() => {
      if(user !== null) {
        operatorService.getOperatorById(user.id)
        .then((response) => {
          if(response.drivers === 0 && response.vehicles === 0) {
            setDrivers([]);
            setVehicles([]);
            setTransactions([]);
            setError("");
          }
          else{
            driverService.getDriversByOperator(
              user.id, 
            ).then((response) => {
              setDrivers(response.reverse());
              setError("");
            }).catch((error) => {
              setMessage(error.response.data || "Failed to load drivers.");
              setError(error);
            }).finally(() => {
              setLoading(false);
            })
    
            vehicleService.getVehiclesByOperator(
              user.id
            ).then((response) => {
              setVehicles(response);
              setError("");
            }).catch((error) => {
              setMessage(error.response.data || "Failed to load vehicles.");
              setError(error);
            }).finally(() => {
              setLoading(false);
            })
    
            transactionService.getTransactionsByOperator(
              user.id
            ).then((response) => {
              setTransactions(response);
              setError("");
            }).catch((error) => {
              setMessage(error.response.data || "Failed to load transactions.");
              setError(error);
            }).finally(() => {
              setLoading(false);
            })
          }
        }).catch((error) => {
          setMessage(error.response.data || "Failed to load operator.");
          setError(error);
        }).finally(() => {
          setLoading(false);
        })
      }
      else{
          window.localStorage.removeItem("ARANGKADA_USER");
          navigate("/", { replace: true })
      }
    }, [user, setMessage, navigate]);

    if (loading) return (<Loading />)

    if (error !== "") return (<ResponseError message={error} />)
  
    return (
      <>
        <Box mt="12px" sx={{ minHeight: "80vh" }}>
          <PageHeader title={"Welcome, " + user?.fullName + "!"} />
          <br></br>
          <Grid container spacing={2} alignItems="center" justifyContent="center" >
            <Grid item xs={12} md={6} lg={3}>
              <DashboardCard title="Total Vehicles" count={vehicles.length}>
                <Commute fontSize="large" color="secondary" />
              </DashboardCard>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
              <DashboardCard title="Vehicles Rented" count={vehicles.filter((vehicle) => vehicle.rentStatus === true).length}>
                <CarRental fontSize="large" color="info" />
              </DashboardCard>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
              <DashboardCard title="Paid Drivers" count={transactions.filter((transaction) => transaction.driverName !== null || transaction.driverName !== "").length}>
                <PriceCheckIcon fontSize="large" color="success" />
              </DashboardCard>
            </Grid>
          </Grid>
          <br></br>
          <Paper sx={{ padding: "12px 24px", display: "flex", flexDirection: "column", alignItems: "center" }}>
            <Typography variant="h5">All Drivers Renting</Typography>
            <div style={{ flexGrow: 1, width: "100%", minHeight: "380px" }}>
              <DataGrid
                autoHeight
                sx={{ minHeight: "369px", '.MuiDataGrid-columnSeparator': { display: 'none' }, '&.MuiDataGrid-root': { border: 'none' } }}
                rows={drivers.filter((driver) => driver.vehicleAssigned !== null)}
                columns={columns}
                getRowId={(row) => row.id}
                initialState={
                  {
                    pagination: {
                      paginationModel: {pageSize: 5}
                    }
                  }
                }
                pageSizeOptions={[5, 10, 15]}
              />
            </div>
          </Paper>
        </Box>
        <Footer name="Kerr Labajo" course="BSCS" section="F1" />
      </>
    );
  }
  
export default HomePage;