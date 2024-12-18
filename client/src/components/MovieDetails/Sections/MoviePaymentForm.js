import React, { useReducer, useState } from 'react';
import { Grid, Typography, TextField, Button, Box, Link, Snackbar } from '@material-ui/core';
import Alert from "@material-ui/lab/Alert";
import { axiosApiInstance as API } from '../../../utils/axiosConfig';
import { useRecoilValue } from 'recoil';
import { userAtom } from '../../../atoms/atoms';

export default function MoviePaymentForm(props) {
    console.log(props);
    const user = useRecoilValue(userAtom);

    const [state, setState] = useReducer((state, newState) => ({ ...state, ...newState }), {
        cardNumber: '',
        expiryDate: '',
        cvv: '',
        nameOnCard: '',
    });

    const handleChange = (event) => {
        setState({ [event.target.name]: event.target.value });
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        // const { cardNumber, expiryDate, cvv, nameOnCard } = state;

        // let body = {
        //     scheduleId: props.scheduleDetail.scheduleId,
        //     seatNumbers: [],
        //     customerId: user.id,
        //     numberOfSeats: props.qty,
        //     totalPrice: props.scheduleDetail?.price * props.qty,
        //     bookingDate: props.scheduleDetail?.date,
        //     paymentDetails: {
        //         cardNumber,
        //         expiryDate,
        //         cvv,
        //         nameOnCard,
        //     },
        // };

        // API.post('bookings', body)
        //     .then((response) => {
        //         if (response.status === 201) {
        //             props.handleSnackbar();
        //         }
        //     })
        //     .catch((error) => alert('Payment failed'));
        console.log('Payment successfull');
        props.handleSnackbar();
            props.handleSnackbarClose();
        // props.handleClose();
    };
;

    return (
        <div>
            {user.isLoggedIn ? (
                <React.Fragment>
                    
                    <Typography variant="h6" gutterBottom>
                        Payment Info
                    </Typography>
                    <Grid container spacing={3}>
                        <Grid item xs={12}>
                            <TextField required id="cardNumber" name="cardNumber" label="Card Number" type="text" fullWidth value={state.cardNumber} onChange={handleChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField required id="expiryDate" name="expiryDate" label="Expiry Date" type="text" fullWidth value={state.expiryDate} onChange={handleChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField required id="cvv" name="cvv" label="CVV" type="text" fullWidth value={state.cvv} onChange={handleChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField required id="nameOnCard" name="nameOnCard" label="Name on Card" type="text" fullWidth value={state.nameOnCard} onChange={handleChange} />
                        </Grid>
                        <Box marginLeft="10px" marginBottom="30px">
                            <Button variant="contained" color="primary" onClick={handleSubmit}>
                                Pay
                            </Button>
                        </Box>
                    </Grid>
                </React.Fragment>
            ) : (
                <div>
                    <Typography variant="h6" color="primary">
                        Login to make a payment
                    </Typography>
                    <Box textAlign="center" marginY="30px">
                        <Link href="/login" underline="none">
                            <Button variant="contained" color="primary">
                                Login
                            </Button>
                        </Link>
                    </Box>
                </div>
            )}
        </div>
    );
}



