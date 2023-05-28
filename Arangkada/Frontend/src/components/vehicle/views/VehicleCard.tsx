import { Button, Card, CardActions, CardContent, CardHeader, Divider, Stack, Typography } from "@mui/material";
import { Vehicle } from "../../../services/dataTypes";
import RouteIcon from '@mui/icons-material/Route';
import { Link, } from "react-router-dom";


type VehicleCardProps = {
  vehicles: Vehicle,
}

const VehicleCard = ({ vehicles }: VehicleCardProps) => {
  return (
    <>
      <Card>
        <CardHeader
          title={vehicles.plateNumber}
          subheader={
            <Stack spacing={0.5} direction="row" alignItems="center">
              <RouteIcon /> <Typography variant="body1">{vehicles.bodyType}</Typography>
            </Stack>
          }
        />
        <Divider />
        <CardContent>
          <Typography
            variant="body1">Make: <b>{vehicles.make}</b>
          </Typography>
          <Typography
            variant="body1">Rent Status: <b>{vehicles.rentStatus === true ? "Rented" : "Not Rented"}</b>
          </Typography>
          <Typography
            variant="body1">Rent Fee: <b>{vehicles.rentFee}</b>
          </Typography>
          <Typography
            variant="body1">CR Number: <b>{vehicles.crNumber}</b>
          </Typography>
          <Typography
            variant="body1">Vehicle Id: <b>{vehicles.id}</b>
          </Typography>
        </CardContent>
        <CardActions>
          <Stack direction={{ xs: "column-reverse", md: "row" }} width="100%" spacing={{ xs: 2, md: 3 }} justifyContent="end" padding={1}>
            <Link to={vehicles.id + "/remove"} style={{ textDecoration: 'none' }}>
              <Button
                size="small"
                variant="contained"
                className='remove'
                color="error"
                sx={{ width: "150px" }}>
                Remove
              </Button>
            </Link>
            <Link to={vehicles.id + "/edit"} style={{ textDecoration: 'none' }}>
              <Button
                size="small"
                variant="contained"
                sx={{ width: "150px" }}>
                Update
              </Button>
            </Link>
          </Stack>
        </CardActions>
      </Card>
    </>
  );
}
export default VehicleCard;