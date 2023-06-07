import { Button, Card, CardActions, CardContent, CardHeader, Divider, Stack, Typography } from "@mui/material";
import { Driver } from "../../../services/dataTypes";
import RouteIcon from '@mui/icons-material/Route';
import { Link, } from "react-router-dom";

type DriverCardProps = {
  driver: Driver,
}

const DriverCard = ({ driver }: DriverCardProps) => {
  return (
    <>
      <Card>
        <CardHeader
          title={driver.fullName}
          subheader={
          <Stack spacing={0.5} direction="row" alignItems="center">
            <RouteIcon /> <Typography variant="body1">{driver.vehicleAssigned !== null ? driver.vehicleAssigned : "Not Currently Renting"}</Typography>
          </Stack>
          }
          action={
            driver.vehicleAssigned === null && (
              <Link to={driver.id + "/assign"} style={{ textDecoration: 'none' }}>
                <Button
                  variant="text"
                  fullWidth
                  size="medium"
                  sx={{ color: "green", marginTop: 5, marginRight: 10}} >
                  Assign Driver
                </Button>
              </Link>
            )
          }
          
        />
        <Divider />
        <CardContent>
          <Typography
            variant="body1">Contact Number: <b>{driver.contactNumber}</b>
          </Typography>
          <Typography
            variant="body1">Address: <b>{driver.address}</b>
          </Typography>
          <Typography
            variant="body1">DL Expiration Date: <b>{driver.expirationDate}</b>
          </Typography>
          <Typography
            variant="body1">License Number: <b>{driver.licenseNumber}</b>
          </Typography>
          <Typography
            variant="body1">DL Codes: <b>{driver.dlCodes}</b>
          </Typography>
          <Typography
            variant="body1">Category: <b>{driver.category}</b>
          </Typography>
        </CardContent>
        <CardActions>
          <Stack direction={{ xs: "column-reverse", md: "row" }} width="100%" spacing={{ xs: 2, md: 3 }} justifyContent="end" padding={1}>
            <Link to={driver.id + "/remove"} style={{ textDecoration: 'none' }}>
              <Button
                size="small"
                variant="contained"
                className='remove'
                color="error"
                sx={{ width: "150px" }}>
                Remove
              </Button>
            </Link>
            <Link to={driver.id + "/edit"} style={{ textDecoration: 'none' }}>
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
export default DriverCard;