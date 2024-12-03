import React, { useReducer } from 'react';
import { Button, CssBaseline, TextField, Container, CircularProgress, Backdrop, Grid, Input, InputLabel } from '@material-ui/core';
import useStyles from './MovieFormStyles';
import Alert from '@material-ui/lab/Alert';
import { axiosApiInstance as API } from '../../../../utils/axiosConfig';
import { useEffect, useState } from 'react';
import { MenuItem, Select } from '@material-ui/core';

export default function MovieForm(props) {
    const classes = useStyles();

    const [state, setState] = useReducer((state, newState) => ({ ...state, ...newState }), {
        name: '',
        description: '',
        genreId: '',
        language: '',
        duration: 0,
        // playingDate: "",
        // playingTime: "",
        // ticketPrice: 0,
        rating: 0,
        genre: '',
        trailorUrl: '',
        image: '',
        isLoading: false,
        error: '',
    });

    const {
        name,
        description,
        genreId,
        language,
        duration,
        // playingDate,
        // playingTime,
        // ticketPrice,
        rating,
        genre,
        trailorUrl,
        image,
        error,
        isLoading,
    } = state;

    const isFormValid = () => {
        console.log({ name, description, genreId, language, duration, rating, genre, trailorUrl, image });
        console.log(
            name &&
                description &&
                genreId &&
                language &&
                duration &&
                // playingDate &&
                // playingTime &&
                // ticketPrice &&
                rating &&
                // genre &&
                trailorUrl &&
                image
        );
        return (
            name &&
            description &&
            genreId &&
            language &&
            duration &&
            // playingDate &&
            // playingTime &&
            // ticketPrice &&
            rating &&
            // genre &&
            trailorUrl &&
            image
        );
    };

    const [genres, setGenres] = useState([]);

    useEffect(() => {
        API.get('genres')
            .then((response) => {
                setGenres(response.data);
            })
            .catch((error) => {
                console.log(error);
                setState({ error: 'Failed to fetch theatres' });
            });
    }, []);

    const handleChange = (event) => {
        setState({ [event.target.name]: event.target.value });
    };

    const handleImage = (event) => {
        setState({
            image: event.target.files[0],
        });
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        setState({ isLoading: true });

        let formData = new FormData();
        formData.append('Name', name);
        formData.append('Description', description);
        formData.append('GenreId', genreId);
        formData.append('Language', language);
        formData.append('Duration', duration);
        // formData.append("PlayingDate", playingDate);
        // formData.append("PlayingTime", playingTime);
        // formData.append("TicketPrice", ticketPrice);
        formData.append('Rating', rating);
        formData.append('Genre', genre);
        formData.append('TrailorUrl', trailorUrl);
        formData.append('Image', image, image.name);

        API.post('movies', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
            .then((response) => {
                if (response.status === 201) {
                    props.handleSnackbarOpen();
                    props.handleDialogClose();
                }
            })
            .catch((error) => {
                console.log(error);
                setState({ error: 'Failed to add the movie' });
            })
            .finally(() => {
                setState({ isLoading: false });
            });
    };

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline />
            <div className={classes.paper}>
                {error && (
                    <Alert className={classes.alert} severity="error">
                        {error}
                    </Alert>
                )}
                <form className={classes.form} noValidate onSubmit={handleSubmit}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="name" label="Movie Name" name="name" type="text" value={name} onChange={handleChange} />
                        </Grid>

                        {/* <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="genre" label="Genre" type="text" name="genre" value={genre} onChange={handleChange} />
                        </Grid> */}

                        <Grid item xs={12}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="description" name="description" label="Description" type="text" value={description} onChange={handleChange} />
                        </Grid>

                        {/* create  a select input for theatreId. get the data from the API. from api we get id and name. display name in the select input */}

                        <Grid item xs={6} sm={6}>
                            <InputLabel id="genreId-label">Genre</InputLabel>
                            <Select labelId="genreId-label" id="genreId" name="genreId" value={genreId} onChange={handleChange} fullWidth variant="outlined">
                                {genres.map((theatre) => (
                                    <MenuItem key={theatre.id} value={theatre.id}>
                                        {theatre.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </Grid>

                        <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="language" label="Language" type="text" name="language" value={language} onChange={handleChange} />
                        </Grid>

                        <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth type="number" id="duration" label="Duration hrs" name="duration" value={duration} placeholder="Ex) 1h 53m" onChange={handleChange} />
                        </Grid>

                        {/* <Grid item xs={12} sm={6}>
                            <TextField
                                id="date"
                                label="Playing Date"
                                type="date"
                                name="playingDate"
                                value={playingDate}
                                fullWidth
                                onChange={handleChange}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        </Grid> */}

                        {/* <Grid item xs={12} sm={6}>
                            <TextField
                                id="time"
                                label="Playing Time"
                                name="playingTime"
                                type="time"
                                value={playingTime}
                                fullWidth
                                onChange={handleChange}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                                inputProps={{
                                    step: 300, // 5 min
                                }}
                            />
                        </Grid> */}

                        {/* <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="ticketPrice" label="Ticket Price" type="number" name="ticketPrice" value={ticketPrice} onChange={handleChange} placeholder="Ex) 12.99" />
                        </Grid> */}

                        <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="rating" label="Rating" type="number" name="rating" value={rating} onChange={handleChange} placeholder="Ex) 8.8" />
                        </Grid>

                        <Grid item xs={12}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="trailorUrl" label="Trailor URL" type="text" name="trailorUrl" value={trailorUrl} onChange={handleChange} />
                        </Grid>

                        <Grid item xs={12}>
                            <InputLabel>Upload Poster</InputLabel>
                            <Input type="file" onChange={handleImage} />
                        </Grid>
                    </Grid>
                    <Button type="submit" fullWidth variant="contained" color="primary" className={classes.submit} disabled={!isFormValid()}>
                        Submit
                    </Button>
                </form>
            </div>
            <Backdrop className={classes.backdrop} open={isLoading}>
                <CircularProgress color="inherit" />
            </Backdrop>
        </Container>
    );
}
