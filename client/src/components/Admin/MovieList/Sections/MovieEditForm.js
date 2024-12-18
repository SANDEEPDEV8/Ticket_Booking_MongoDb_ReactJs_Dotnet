import React, { useState, useEffect, useReducer } from 'react';
import { Button, CssBaseline, TextField, Container, CircularProgress, Backdrop, Grid, Input, InputLabel } from '@material-ui/core';
import useStyles from './MovieFormStyles';
import Alert from '@material-ui/lab/Alert';
import { axiosApiInstance as API } from '../../../../utils/axiosConfig';
import axios from 'axios';
import { MenuItem, Select } from '@material-ui/core';

export default function MovieEditForm(props) {
    const classes = useStyles();
    const baseURL = 'https://localhost:5001/api/';

    useEffect(() => {
        axios.get(`${baseURL}movies/${props.movieId}`).then((response) => {
            if (response.status === 200) {
                setState({
                    name: response.data.name,
                    description: response.data.description,
                    language: response.data.language,
                    duration: response.data.duration,
                    ticketPrice: response.data.ticketPrice,
                    rating: response.data.rating,
                    genreId: response.data.genreId,
                    actorName: response.data.actorName,
                    directorName: response.data.directorName,
                    trailorUrl: response.data.trailorUrl,
                });
            }
        });
    }, [props.movieId]);

    const [state, setState] = useReducer((state, newState) => ({ ...state, ...newState }), {
        name: '',
        description: '',
        theatreId: '',
        language: '',
        duration: '',
        playingDate: '',
        playingTime: '',
        ticketPrice: 0,
        rating: 0,
        genreId: '',
        actorName: '',
        directorName: '',
        trailorUrl: '',
        image: '',
        isLoading: false,
        error: '',
    });

    const { name, description, language, duration, rating, genreId, actorName, directorName, trailorUrl, image } = state;

    const [genres, setGenres] = useState([]);
    const isFormValid = () => {
        return name && description && language && duration && rating && genreId && actorName && directorName && trailorUrl && image;
    };

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
        formData.append('Language', language);
        formData.append('Duration', duration);
        formData.append('Rating', rating);
        formData.append('GenreId', genreId);
        formData.append('ActorName', actorName);
        formData.append('DirectorName', directorName);
        formData.append('TrailorUrl', trailorUrl);
        formData.append('Image', image, image.name);

        API.put(`movies/${props.movieId}`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
            .then((response) => {
                if (response.status === 200) {
                    props.handleEditSnackbarOpen();
                    props.handleEditDialogClose();
                    setTimeout(() => {
                        window.location.reload();
                    }, 3000);
                }
            })
            .catch((error) => {
                console.log(error);
                setState({ error: 'Failed to update the movie' });
            })
            .finally(() => {
                setState({ isLoading: false });
            });
    };

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline />
            <div className={classes.paper}>
                {state.error && (
                    <Alert className={classes.alert} severity="error">
                        {state.error}
                    </Alert>
                )}
                <form className={classes.form} noValidate onSubmit={handleSubmit}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="name" label="Movie Name" name="name" type="text" value={state.name} onChange={handleChange} />
                        </Grid>

                        <Grid item xs={12}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="description" name="description" label="Description" type="text" value={state.description} onChange={handleChange} />
                        </Grid>
                        <Grid item xs={6} sm={6}>
                            <InputLabel id="genreId-label">Genre</InputLabel>
                            <Select labelId="genreId-label" id="genreId" name="genreId" value={genreId} onChange={handleChange} fullWidth variant="outlined">
                                {genres.map((genre) => (
                                    <MenuItem key={genre.id} value={genre.id}>
                                        {genre.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </Grid>

                        {/* <Grid item xs={12}>
                            <InputLabel id="theatreId-label">Theatre</InputLabel>
                            <Select labelId="theatreId-label" id="theatreId" name="theatreId" value={theatreId} onChange={handleChange} fullWidth variant="outlined">
                                {theatres.map((theatre) => (
                                    <MenuItem key={theatre.id} value={theatre.id}>
                                        {theatre.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </Grid> */}



                        <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="language" label="Language" type="text" name="language" value={state.language} onChange={handleChange} />
                        </Grid>

                        <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth type="text" id="duration" label="Duration" name="duration" value={state.duration} placeholder="Ex) 1h 53m" onChange={handleChange} />
                        </Grid>

                        {/* <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="ticketPrice" label="Ticket Price" type="number" name="ticketPrice" value={state.ticketPrice} onChange={handleChange} placeholder="Ex) 12.99" />
                        </Grid> */}

                        {/* <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="rating" label="Rating" type="number" name="rating" value={state.rating} onChange={handleChange} placeholder="Ex) 8.8" />
                        </Grid> */}

                    
                        <Grid item xs={6} sm={6}>
                        <TextField
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            select
                            id="rating"
                            label="Rating"
                            name="rating"
                            value={rating}
                            onChange={handleChange}
                            SelectProps={{
                            native: true,
                            }}
                        >
                            {/* <option value="" disabled>
                            Select a Rating
                            </option> */}
                            <option value="G">G (General Audiences)</option>
                            <option value="PG">PG (Parental Guidance Suggested)</option>
                            <option value="PG-13">PG-13 (Parents Strongly Cautioned)</option>
                            <option value="R">R (Restricted)</option>
                            <option value="NC-17">NC-17 (Adults Only)</option>
                        </TextField>
                        </Grid>

                        <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="actorName" label="Actor Name" type="text" name="actorName" value={actorName} onChange={handleChange} />

                        </Grid>
                        <Grid item xs={6} sm={6}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="directorName" label="Director Name" type="text" name="directorName" value={directorName} onChange={handleChange} />
                        </Grid>
                             


                        <Grid item xs={12}>
                            <TextField variant="outlined" margin="normal" required fullWidth id="trailorUrl" label="Trailor URL" type="text" name="trailorUrl" value={state.trailorUrl} onChange={handleChange} />
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
            <Backdrop className={classes.backdrop} open={state.isLoading}>
                <CircularProgress color="inherit" />
            </Backdrop>
        </Container>
    );
}
